using Core.Enums;
using Core.Utils;
using Core.Utils.Caching;
using Core.Utils.CriticalData;
using Core.Utils.HttpContextManager;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Globalization;

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


        // ############## Localization ##############
        services.AddLocalization(options =>
        {
            options.ResourcesPath = "Utils/Localization";
        });
        services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new[]
            {
                new CultureInfo(Languages.Turkish.GetDescription()),
                new CultureInfo(Languages.English.GetDescription()),
                new CultureInfo(Languages.Russian.GetDescription())
            };

            options.DefaultRequestCulture = new RequestCulture(Languages.Turkish.GetDescription());
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;

            options.RequestCultureProviders =
            [
                new CookieRequestCultureProvider(),
                new QueryStringRequestCultureProvider(),
                new AcceptLanguageHeaderRequestCultureProvider(),
            ];
        });

        return services;
    }
}