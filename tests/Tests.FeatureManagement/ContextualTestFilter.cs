// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
//
using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;
using System;
using System.Threading.Tasks;

namespace Tests.FeatureManagement
{
    class ContextualTestFilter : IContextualFeatureFilter<IAccountContext, IConfiguration>
    {
        public Func<IFeatureFilterEvaluationContext<IConfiguration>, IAccountContext, bool> ContextualCallback { get; set; }

        public Task<bool> EvaluateAsync(IFeatureFilterEvaluationContext<IConfiguration> context, IAccountContext accountContext)
        {
            return Task.FromResult(ContextualCallback?.Invoke(context, accountContext) ?? false);
        }
    }
}
