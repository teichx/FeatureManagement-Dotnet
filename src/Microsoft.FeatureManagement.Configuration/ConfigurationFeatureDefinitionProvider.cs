// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
//
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.FeatureManagement.Configuration
{
    /// <summary>
    /// A feature definition provider that pulls feature definitions from the .NET Core <see cref="IConfiguration"/> system.
    /// </summary>
    sealed class ConfigurationFeatureDefinitionProvider : IFeatureDefinitionProvider<IConfiguration>, IDisposable, IFeatureDefinitionProviderCacheable
    {
        //
        // IFeatureDefinitionProviderCacheable interface is only used to mark this provider as cacheable. This allows our test suite's
        // provider to be marked for caching as well.

        private const string FeatureFiltersSectionName = "EnabledFor";
        private const string RequirementTypeKeyword = "RequirementType";
        private readonly IConfiguration _configuration;
        private readonly ConcurrentDictionary<string, IFeatureDefinition<IConfiguration>> _definitions;
        private IDisposable _changeSubscription;
        private int _stale = 0;

        public ConfigurationFeatureDefinitionProvider(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _definitions = new ConcurrentDictionary<string, IFeatureDefinition<IConfiguration>>();

            _changeSubscription = ChangeToken.OnChange(
                () => _configuration.GetReloadToken(),
                () => _stale = 1);
        }

        public void Dispose()
        {
            _changeSubscription?.Dispose();

            _changeSubscription = null;
        }

        public Task<IFeatureDefinition<IConfiguration>> GetFeatureDefinitionAsync(string featureName)
        {
            if (featureName == null)
            {
                throw new ArgumentNullException(nameof(featureName));
            }

            if (Interlocked.Exchange(ref _stale, 0) != 0)
            {
                _definitions.Clear();
            }

            //
            // Query by feature name
            var definition = _definitions.GetOrAdd(featureName, (name) => ReadFeatureDefinition(name));

            return Task.FromResult(definition);
        }

        //
        // The async key word is necessary for creating IAsyncEnumerable.
        // The need to disable this warning occurs when implementaing async stream synchronously.
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async IAsyncEnumerable<IFeatureDefinition<IConfiguration>> GetAllFeatureDefinitionsAsync()
#pragma warning restore CS1998
        {
            if (Interlocked.Exchange(ref _stale, 0) != 0)
            {
                _definitions.Clear();
            }

            //
            // Iterate over all features registered in the system at initial invocation time
            foreach (IConfigurationSection featureSection in GetFeatureDefinitionSections())
            {
                //
                // Underlying IConfigurationSection data is dynamic so latest feature definitions are returned
                yield return _definitions.GetOrAdd(featureSection.Key, (_) => ReadFeatureDefinition(featureSection));
            }
        }

        private IFeatureDefinition<IConfiguration> ReadFeatureDefinition(string featureName)
        {
            IConfigurationSection configuration = GetFeatureDefinitionSections()
                                                    .FirstOrDefault(section => section.Key.Equals(featureName, StringComparison.OrdinalIgnoreCase));

            if (configuration == null)
            {
                return null;
            }

            return ReadFeatureDefinition(configuration);
        }

        private IFeatureDefinition<IConfiguration> ReadFeatureDefinition(IConfigurationSection configurationSection)
        {
            /*

            We support

            myFeature: {
              enabledFor: [ "myFeatureFilter1", "myFeatureFilter2" ]
            },
            myDisabledFeature: {
              enabledFor: [  ]
            },
            myFeature2: {
              enabledFor: "myFeatureFilter1;myFeatureFilter2"
            },
            myDisabledFeature2: {
              enabledFor: ""
            },
            myFeature3: "myFeatureFilter1;myFeatureFilter2",
            myDisabledFeature3: "",
            myAlwaysEnabledFeature: true,
            myAlwaysDisabledFeature: false // removing this line would be the same as setting it to false
            myAlwaysEnabledFeature2: {
              enabledFor: true
            },
            myAlwaysDisabledFeature2: {
              enabledFor: false
            },
            myAllRequiredFilterFeature: {
                requirementType: "all"
                enabledFor: [ "myFeatureFilter1", "myFeatureFilter2" ],
            },

            */

            RequirementType requirementType = RequirementType.Any;

            var enabledFor = new List<IFeatureFilterEnabledFor<IConfiguration>>();

            string val = configurationSection.Value; // configuration[$"{featureName}"];

            if (string.IsNullOrEmpty(val))
            {
                val = configurationSection[FeatureFiltersSectionName];
            }

            if (!string.IsNullOrEmpty(val) && bool.TryParse(val, out bool result) && result)
            {
                //
                //myAlwaysEnabledFeature: true
                // OR
                //myAlwaysEnabledFeature: {
                //  enabledFor: true
                //}
                enabledFor.Add(new FeatureFilterEnabledFor<IConfiguration>
                {
                    Name = "AlwaysOn"
                });
            }
            else
            {
                string rawRequirementType = configurationSection[RequirementTypeKeyword];

                //
                // If requirement type is specified, parse it and set the requirementType variable
                if (!string.IsNullOrEmpty(rawRequirementType) && !Enum.TryParse(rawRequirementType, ignoreCase: true, out requirementType))
                {
                    throw new FeatureManagementException(
                        FeatureManagementError.InvalidConfigurationSetting,
                        $"Invalid requirement type '{rawRequirementType}' for feature '{configurationSection.Key}'.");
                }

                IEnumerable<IConfigurationSection> filterSections = configurationSection.GetSection(FeatureFiltersSectionName).GetChildren();

                foreach (IConfigurationSection section in filterSections)
                {
                    //
                    // Arrays in json such as "myKey": [ "some", "values" ]
                    // Are accessed through the configuration system by using the array index as the property name, e.g. "myKey": { "0": "some", "1": "values" }
                    if (int.TryParse(section.Key, out int i) && !string.IsNullOrEmpty(section[nameof(IFeatureFilterEnabledFor<IConfiguration>.Name)]))
                    {
                        enabledFor.Add(new FeatureFilterEnabledFor<IConfiguration>()
                        {
                            Name = section[nameof(IFeatureFilterEnabledFor<IConfiguration>.Name)],
                            Parameters = new ConfigurationWrapper(section.GetSection(nameof(IFeatureFilterEnabledFor<IConfiguration>.Parameters)))
                        });
                    }
                }
            }

            return new FeatureDefinition<IConfiguration>()
            {
                Name = configurationSection.Key,
                EnabledFor = enabledFor,
                RequirementType = requirementType
            };
        }

        private IEnumerable<IConfigurationSection> GetFeatureDefinitionSections()
        {
            const string FeatureManagementSectionName = "FeatureManagement";

            if (_configuration.GetChildren().Any(s => s.Key.Equals(FeatureManagementSectionName, StringComparison.OrdinalIgnoreCase)))
            {
                //
                // Look for feature definitions under the "FeatureManagement" section
                return _configuration.GetSection(FeatureManagementSectionName).GetChildren();
            }
            else
            {
                return _configuration.GetChildren();
            }
        }
    }
}
