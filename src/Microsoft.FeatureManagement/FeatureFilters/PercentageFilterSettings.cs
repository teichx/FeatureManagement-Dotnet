﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
//
namespace Microsoft.FeatureManagement.FeatureFilters
{
    /// <summary>
    /// The settings that are used to configure the <see cref="PercentageFilter"/> feature filter.
    /// </summary>
    public class PercentageFilterSettings : IPercentageFilterSettings
    {
        public int Value { get; set; } = -1;
    }
}
