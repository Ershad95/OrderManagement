using Application.Repository;
using Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;
using ServiceCollector.Abstractions;

namespace Infastraucture.EfCore;

public class ServiceRegistration : IServiceDiscovery
{
    public void AddServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUserRepository, UserRepository>();
        serviceCollection.AddScoped<IOrderRepository, OrderRepository>();
        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}