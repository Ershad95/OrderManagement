using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using ServiceCollector.Abstractions;

namespace Application;

public class ServiceRegistration : IServiceDiscovery
{
    public void AddServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ICurrentUserService, CurrentUserService>();
    }
}