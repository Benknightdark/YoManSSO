using Microsoft.Extensions.DependencyInjection;
using OAthLib.Services;
using OAthLib.Services.Helpers;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;

namespace OAthLib
{
    public static class SSOServiceCollections
    {
        #region IAsyncPolicy function

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        #endregion IAsyncPolicy function

        public static void AddSSOConfig(this IServiceCollection services)
        {
            services.AddScoped<ConfigService>();
        }

        public static void AddLine(this IServiceCollection services)
        {
            services.AddHttpClient<LineService>()
                 .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(10))
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy()); ;
        }
        public static void AddFB(this IServiceCollection services)
        {
            services.AddHttpClient<FBService>()
                 .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(10))
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy()); ;
        }
        public static void AddGoogle(this IServiceCollection services)
        {
            services.AddHttpClient<GoogleService>()
                 .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(10))
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy()); ;
        }

        public static void AddLinkedIn(this IServiceCollection services)
        {
            services.AddHttpClient<LinkedInService>()
                 .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(10))
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy()); ;
        }
         public static void AddMS(this IServiceCollection services)
        {
            services.AddHttpClient<MSService>()
                 .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(10))
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy()); ;
        }
    }
}