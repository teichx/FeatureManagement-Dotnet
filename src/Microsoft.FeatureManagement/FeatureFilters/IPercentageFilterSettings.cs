// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
//
namespace Microsoft.FeatureManagement.FeatureFilters
{
    /// <summary>
    /// The settings that are used to configure the <see cref="IPercentageFilterSettings"/> feature filter.
    /// </summary>
    public interface IPercentageFilterSettings
    {
        /// <summary>
        /// A value between 0 and 100 specifying the chance that a feature configured to use the <see cref="IPercentageFilterSettings"/> should be enabled.
        /// </summary>
        public int Value { get; set; }
    }
}
