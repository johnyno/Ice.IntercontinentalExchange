using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreRateLimit;
using Microsoft.Extensions.DependencyInjection;

namespace IntercontinentalExchange.Host.Extntions
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddInMemoryRateLimiting(this IServiceCollection services)
        {
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IClientPolicyStore, MemoryCacheClientPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            return services;
        }
    }

}