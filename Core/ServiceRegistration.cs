using Core.Utils.Caching;
using Core.Utils.CriticalData;
using Core.Utils.HttpContextManager;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Core;

public static class ServiceRegistration
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddSingleton<HttpContextManager>();

        JsonConvert.DefaultSettings = () => new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            MaxDepth = 7,
            ContractResolver = new IgnoreCriticalDataResolver()
        };

        #region Distributed Cache In Memory
        services.AddDistributedMemoryCache();
        // services.AddStackExchangeRedisCache(options =>
        // {
        //     options.Configuration = configuration["Redis:ConnectionString"];
        // });
        services.AddSingleton<ICacheService, CacheService>();
        #endregion

        return services;
    }
}
