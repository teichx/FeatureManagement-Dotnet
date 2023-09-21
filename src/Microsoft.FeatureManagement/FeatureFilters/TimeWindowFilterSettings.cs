// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
//
using System;

namespace Microsoft.FeatureManagement.FeatureFilters
{
    /// <summary>
    /// The settings that are used to configure the <see cref="TimeWindowFilterSettings"/> feature filter.
    /// </summary>
    public class TimeWindowFilterSettings : ITimeWindowFilterSettings
    {
        /// <summary>
        /// An optional start time used to determine when a feature configured to use the <see cref="TimeWindowFilterSettings"/> feature filter should be enabled.
        /// If no start time is specified the time window is considered to have already started.
        /// </summary>
        public virtual DateTimeOffset? Start { get; set; } // E.g. "Wed, 01 May 2019 22:59:30 GMT"

        /// <summary>
        /// An optional end time used to determine when a feature configured to use the <see cref="TimeWindowFilterSettings"/> feature filter should be enabled.
        /// If no end time is specified the time window is considered to never end.
        /// </summary>
        public virtual DateTimeOffset? End { get; set; } // E.g. "Wed, 01 May 2019 23:00:00 GMT"
    }
}
