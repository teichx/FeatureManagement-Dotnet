// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
//
namespace Microsoft.FeatureManagement
{
    /// <summary>
    /// A context used by <see cref="IFeatureFilter{TParameters}"/> to gain insight into what feature is being evaluated and the parameters needed to check whether the feature should be enabled.
    /// </summary>
    public interface IFeatureFilterEvaluationContext<TParameters>
    {
        /// <summary>
        /// The name of the feature being evaluated.
        /// </summary>
        public string FeatureName { get; set; }

        /// <summary>
        /// The settings provided for the feature filter to use when evaluating whether the feature should be enabled.
        /// </summary>
        public TParameters Parameters { get; set; }
        /// <summary>
        /// A settings object, if any, that has been pre-bound from <see cref="Parameters"/>.
        /// The settings are made available for <see cref="IFeatureFilter{TParameters}"/>s that implement />.
        /// </summary>
        public object Settings { get; set; }
    }
}
