using Application.Events;
using MassTransit;

namespace WebHost.Extensions;

public static class MassTransientExtension
{
    public static void MassTransit(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddMassTransit(configurator =>
        {
            configurator.UsingInMemory((context, cfg) => { cfg.ConfigureEndpoints(context); });

            configurator.AddConsumer(typeof(Infrastructure.EventConsumer.OrderCreated));
            configurator.AddConsumer(typeof(Infrastructure.EventConsumer.OrderDeleted));
            configurator.AddConsumer(typeof(Infrastructure.EventConsumer.OrderUpdated));
        });
    }
}