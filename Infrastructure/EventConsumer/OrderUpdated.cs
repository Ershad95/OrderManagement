using MassTransit;

namespace Infrastructure.EventConsumer;

public class OrderUpdated : IConsumer<OrderUpdated>
{
    public Task Consume(ConsumeContext<OrderUpdated> context)
    {
        return Task.CompletedTask;
    }
}