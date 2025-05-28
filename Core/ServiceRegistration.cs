using Core.Utils.Caching;
using Core.Utils.RequestInfoProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Core;

public static class ServiceRegistration
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddSingleton<RequestInfoProvider>();

        JsonConvert.DefaultSettings = () => new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            MaxDepth = 7,
        };

        #region Distributed Cache 
        services.AddDistributedMemoryCache(); // In Memory
        // services.AddStackExchangeRedisCache(options =>
        // {
        //     options.Configuration = configuration["Redis:ConnectionString"];
        // });
        services.AddSingleton<ICacheService, CacheService>();
        #endregion

        return services;
    }
}
