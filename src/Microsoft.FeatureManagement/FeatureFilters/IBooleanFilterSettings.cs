// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
//
namespace Microsoft.FeatureManagement.FeatureFilters
{
    /// <summary>
    /// The settings that are used to configure the <see cref="IBooleanFilterSettings"/> feature filter.
    /// </summary>
    public interface IBooleanFilterSettings
    {
        /// <summary>
        /// When IsFullyEnabled is null, this property is ignored
        /// When IsFullyEnabled is boolean, EnabledFor considers all object as current value
        /// </summary>
        public bool? IsFullyEnabled { get; set; }
    }
}
