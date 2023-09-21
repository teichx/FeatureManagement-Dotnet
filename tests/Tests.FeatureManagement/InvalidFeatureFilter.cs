// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
//
using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;
using System.Threading.Tasks;

namespace Tests.FeatureManagement
{
    //
    // Cannot implement more than one IFeatureFilter interface
    class InvalidFeatureFilter : IContextualFeatureFilter<IAccountContext, IConfiguration>, IContextualFeatureFilter<object, IConfiguration>
    {
        public Task<bool> EvaluateAsync(IFeatureFilterEvaluationContext<IConfiguration> context, IAccountContext accountContext)
        {
            return Task.FromResult(false);
        }

        public Task<bool> EvaluateAsync(IFeatureFilterEvaluationContext<IConfiguration> featureFilterContext, object appContext)
        {
            return Task.FromResult(false);
        }
    }

    //
    // Cannot implement more than one IFeatureFilter interface
    class InvalidFeatureFilter2 : IFeatureFilter<IConfiguration>, IContextualFeatureFilter<object, IConfiguration>
    {
        public Task<bool> EvaluateAsync(IFeatureFilterEvaluationContext<IConfiguration> featureFilterContext, object appContext)
        {
            return Task.FromResult(false);
        }

        public Task<bool> EvaluateAsync(IFeatureFilterEvaluationContext<IConfiguration> context)
        {
            return Task.FromResult(false);
        }
    }
}
