using Application.Repository;
using Application.Services;
using Infrastructure.Repository;
using Infrastructure.Services;

namespace WebHost.Extensions;

public static class ServiceRegistration
{
    public static void CustomServiceRegistration(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ICurrentUserService, CurrentUserService>();
        serviceCollection.AddScoped<IDateTimeService, DateTimeService>();
        serviceCollection.AddScoped<IUserRepository, UserRepository>();
        serviceCollection.AddScoped<IOrderRepository, OrderRepository>();
        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
        serviceCollection.AddSingleton<IEmailService, FakeEmailService>();
        serviceCollection.AddSingleton<ISmsService, FakeSmsService>();
        serviceCollection.AddScoped<IJwtManager, JwtManager>();
    }
}