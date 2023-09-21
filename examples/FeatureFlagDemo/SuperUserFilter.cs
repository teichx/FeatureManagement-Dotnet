// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
//
using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;
using System.Threading.Tasks;

namespace FeatureFlagDemo.FeatureManagement.FeatureFilters
{
    public class SuperUserFilter : IFeatureFilter<IConfiguration>
    {
        public Task<bool> EvaluateAsync(IFeatureFilterEvaluationContext<IConfiguration> context)
        {
            return Task.FromResult(false);
        }
    }
}
