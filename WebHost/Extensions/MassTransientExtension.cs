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

            configurator.AddConsumer(typeof(OrderCreated));
            configurator.AddConsumer(typeof(OrderDeleted));
            configurator.AddConsumer(typeof(OrderUpdated));
        });
    }
}