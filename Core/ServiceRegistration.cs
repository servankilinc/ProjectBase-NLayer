using Core.Utils.Caching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

public static class ServiceRegistration
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

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
