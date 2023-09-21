// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
//
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.FeatureManagement
{
    /// <summary>
    /// The definition of a feature.
    /// </summary>
    public class FeatureDefinition<TParameters> : IFeatureDefinition<TParameters>
    {
        /// <summary>
        /// The name of the feature.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The feature filters that the feature can be enabled for.
        /// </summary>
        public IEnumerable<IFeatureFilterEnabledFor<TParameters>> EnabledFor { get; set; } = Enumerable.Empty<IFeatureFilterEnabledFor<TParameters>>();

        /// <summary>
        /// Determines whether any or all registered feature filters must be enabled for the feature to be considered enabled
        /// The default value is <see cref="RequirementType.Any"/>.
        /// </summary>
        public RequirementType RequirementType { get; set; } = RequirementType.Any;
    }
}
