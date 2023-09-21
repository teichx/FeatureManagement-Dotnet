using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tests.FeatureManagement
{
    class InMemoryFeatureDefinitionProvider : IFeatureDefinitionProvider<IConfiguration>, IFeatureDefinitionProviderCacheable
    {
        private IEnumerable<IFeatureDefinition<IConfiguration>> _definitions;

        public InMemoryFeatureDefinitionProvider(IEnumerable<IFeatureDefinition<IConfiguration>> featureDefinitions)
        {
            _definitions = featureDefinitions ?? throw new ArgumentNullException(nameof(featureDefinitions));
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async IAsyncEnumerable<IFeatureDefinition<IConfiguration>> GetAllFeatureDefinitionsAsync()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            foreach (var definition in _definitions)
            {
                yield return definition;
            }
        }

        public Task<IFeatureDefinition<IConfiguration>> GetFeatureDefinitionAsync(string featureName)
        {
            return Task.FromResult(_definitions.FirstOrDefault(definitions => definitions.Name.Equals(featureName, StringComparison.OrdinalIgnoreCase)));
        }
    }
}
