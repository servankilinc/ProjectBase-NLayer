using DataAccess.Contexts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using DataAccess.Interceptors;

namespace DataAccess;

public static class ServiceRegistration
{
    public static IServiceCollection AddDataAccessServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<AuditInterceptor>();
        services.AddSingleton<ArchiveInterceptor>();
        services.AddSingleton<LogInterceptor>();
        services.AddSingleton<SoftDeleteInterceptor>();
  
        services.AddDbContext<AppDbContext>((serviceProvider, opt) =>
        {
            opt.UseSqlServer(configuration.GetConnectionString("Database"))
                .AddInterceptors(serviceProvider.GetRequiredService<AuditInterceptor>())
                .AddInterceptors(serviceProvider.GetRequiredService<ArchiveInterceptor>())
                .AddInterceptors(serviceProvider.GetRequiredService<LogInterceptor>())
                .AddInterceptors(serviceProvider.GetRequiredService<SoftDeleteInterceptor>());
        });

        return services;
    }
}
