using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using ServiceCollector.Abstractions;

namespace Infrastructure.Services;

public class ServiceRegistration : IServiceDiscovery
{
    public void AddServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IDateTimeService, DateTimeService>();
        serviceCollection.AddSingleton<IEmailService, FakeEmailService>();
        serviceCollection.AddSingleton<ISmsService, FakeSmsService>();
        serviceCollection.AddScoped<IJwtManager, JwtManager>();
    }
}