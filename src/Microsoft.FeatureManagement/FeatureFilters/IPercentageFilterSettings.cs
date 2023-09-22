// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
//
namespace Microsoft.FeatureManagement.FeatureFilters
{
    /// <summary>
    /// Interface for settings that are used to configure the <see cref="PercentageFilter"/> feature filter.
    /// </summary>
    public interface IPercentageFilterSettings : IFeatureFilterSettings
    {
        /// <summary>
        /// A value between 0 and 100 specifying the chance that a feature configured to use the <see cref="PercentageFilter"/> should be enabled.
        /// </summary>
        int Value { get; set; }
    }
}
