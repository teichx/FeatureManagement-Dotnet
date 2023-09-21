// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
//
namespace Microsoft.FeatureManagement.FeatureFilters
{
    /// <summary>
    /// The settings that are used to configure the <see cref="PercentageFilterSettings"/> feature filter.
    /// </summary>
    public class PercentageFilterSettings : IPercentageFilterSettings
    {
        /// <summary>
        /// A value between 0 and 100 specifying the chance that a feature configured to use the <see cref="PercentageFilterSettings"/> should be enabled.
        /// </summary>
        public virtual int Value { get; set; } = -1;
    }
}
