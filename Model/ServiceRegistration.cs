using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Model;

public static class ServiceRegistration
{
    public static IServiceCollection AddModelServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}
