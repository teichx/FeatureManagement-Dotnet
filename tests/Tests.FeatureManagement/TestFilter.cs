// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
//
using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;
using System;
using System.Threading.Tasks;

namespace Tests.FeatureManagement
{
    class TestFilter : IFeatureFilter<IConfiguration>
    {
        public Func<IConfiguration, object> ParametersBinderCallback { get; set; }

        public Func<IFeatureFilterEvaluationContext<IConfiguration>, Task<bool>> Callback { get; set; }

        public object BindParameters(IConfiguration parameters)
        {
            if (ParametersBinderCallback != null)
            {
                return ParametersBinderCallback(parameters);
            }

            return parameters;
        }

        public Task<bool> EvaluateAsync(IFeatureFilterEvaluationContext<IConfiguration> context)
        {
            return Callback?.Invoke(context) ?? Task.FromResult(false);
        }
    }
}
