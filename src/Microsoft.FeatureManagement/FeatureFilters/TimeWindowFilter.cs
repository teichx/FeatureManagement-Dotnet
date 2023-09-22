// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
//
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Microsoft.FeatureManagement.FeatureFilters
{
    /// <summary>
    /// A feature filter that can be used to activate a feature based on a time window.
    /// </summary>
    [FilterAlias(Alias)]
    public class TimeWindowFilter : IFeatureFilter<ITimeWindowFilterSettings>
    {
        private const string Alias = "Microsoft.TimeWindow";
        private readonly ILogger _logger;

        /// <summary>
        /// Creates a time window based feature filter.
        /// </summary>
        /// <param name="loggerFactory">A logger factory for creating loggers.</param>
        public TimeWindowFilter(ILoggerFactory loggerFactory)
            => _logger = loggerFactory.CreateLogger<TimeWindowFilter>();

        /// <summary>
        /// Evaluates whether a feature is enabled based on a configurable time window.
        /// </summary>
        /// <param name="context">The feature evaluation context.</param>
        /// <returns>True if the feature is enabled, false otherwise.</returns>
        public virtual Task<bool> EvaluateAsync(IFeatureFilterEvaluationContext<ITimeWindowFilterSettings> context)
        {
            var start = context.Parameters.Start;
            var end = context.Parameters.End;

            var now = DateTimeOffset.UtcNow;

            if (!start.HasValue && !end.HasValue)
            {
                _logger.LogWarning($"The '{Alias}' feature filter is not valid for feature '{context.FeatureName}'. It must have have specify either '{nameof(context.Parameters.Start)}', '{nameof(context.Parameters.End)}', or both.");

                return Task.FromResult(false);
            }

            return Task.FromResult((!start.HasValue || now >= start.Value) && (!end.HasValue || now < end.Value));
        }
    }
}
