// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
//
using Microsoft.FeatureManagement.FeatureFilters;

namespace Microsoft.FeatureManagement
{
    /// <summary>
    /// The settings that are used to configure the <see cref="IFeatureFlag"/> feature filter.
    /// </summary>
    public interface IFeatureFlag :
        IPercentageFilterSettings,
        ITimeWindowFilterSettings,
        IBooleanFilterSettings,
        ITargetingFilterSettings
    {
    }
}
