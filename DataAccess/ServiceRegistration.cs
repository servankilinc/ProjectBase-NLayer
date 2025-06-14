using DataAccess.Contexts;
using DataAccess.Interceptors;
using DataAccess.UoW;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
