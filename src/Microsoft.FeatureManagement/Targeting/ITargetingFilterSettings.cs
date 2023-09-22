// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
//
#nullable enable
namespace Microsoft.FeatureManagement.FeatureFilters
{
    /// <summary>
    /// The settings that are used to configure the <see cref="ITargetingFilterSettings"/> feature filter.
    /// </summary>
    public interface ITargetingFilterSettings
    {
        /// <summary>
        /// The audience that a feature configured to use the <see cref="ITargetingFilterSettings"/> should be enabled for.
        /// </summary>
        IAudience? Audience { get; set; }
    }
}
