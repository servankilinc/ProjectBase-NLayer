using Core.Utils.Repository.Interceptors;
using DataAccess.Contexts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public static class ServiceRegistration
{
    public static IServiceCollection AddDataAccessServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<CreateInterceptor>();
        services.AddSingleton<UpdateInterceptor>();
        services.AddSingleton<HardDeleteInterceptor>();
        services.AddSingleton<SoftDeleteInterceptor>();

        services.AddDbContext<AppDbContext>((serviceProvider, opt) =>
        {
            opt.UseSqlServer(configuration.GetConnectionString("Database"))
                .AddInterceptors(serviceProvider.GetRequiredService<CreateInterceptor>())
                .AddInterceptors(serviceProvider.GetRequiredService<UpdateInterceptor>())
                .AddInterceptors(serviceProvider.GetRequiredService<HardDeleteInterceptor>())
                .AddInterceptors(serviceProvider.GetRequiredService<SoftDeleteInterceptor>());
        });

        return services;
    }
}
