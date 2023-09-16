using MassTransit;

namespace WebHost.Extensions;

public static class MassTransientExtension
{
    public static void MassTransit(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddMassTransit(configurator =>
        {
            configurator.UsingInMemory((context, cfg) => { cfg.ConfigureEndpoints(context); });

            configurator.AddConsumer(typeof(Infrastructure.Services.EventConsumer.OrderCreated));
            configurator.AddConsumer(typeof(Infrastructure.Services.EventConsumer.OrderDeleted));
            configurator.AddConsumer(typeof(Infrastructure.Services.EventConsumer.OrderUpdated));
        });
    }
}