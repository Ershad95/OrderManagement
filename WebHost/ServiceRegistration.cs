using Application.Repository;
using Application.Services;
using Infrastructure;

namespace WebHost;

public static class ServiceRegistration
{
    public static void CustomServiceRegistration(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUserService, UserService>();
        serviceCollection.AddScoped<IDateTimeService, DateTimeService>();
        serviceCollection.AddScoped<IUserRepository, UserRepository>();
        serviceCollection.AddScoped<IOrderRepository, OrderRepository>();
        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
        serviceCollection.AddSingleton<IEmailService, FakeEmailService>();
        serviceCollection.AddSingleton<ISmsService, FakeSmsService>();
        serviceCollection.AddSingleton<IJwtManager, JwtManager>();
    }
}