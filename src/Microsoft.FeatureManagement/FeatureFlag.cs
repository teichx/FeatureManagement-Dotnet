// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
//
using System;
using Microsoft.FeatureManagement.FeatureFilters;

#nullable enable
namespace Microsoft.FeatureManagement
{
    public struct FeatureFlag : IFeatureFlag
    {
        public bool? IsFullyEnabled { get; set; }
        public DateTimeOffset? Start { get; set; }
        public DateTimeOffset? End { get; set; }
        public int? Value { get; set; }
        public IAudience? Audience { get; set; }
    }
}
