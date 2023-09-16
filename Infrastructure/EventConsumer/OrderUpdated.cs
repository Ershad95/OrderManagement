using MassTransit;

namespace Infrastructure.EventConsumer;

public class OrderUpdated : IConsumer<Application.Events.OrderUpdated>
{
    public Task Consume(ConsumeContext<Application.Events.OrderUpdated> context)
    {
        
        return Task.CompletedTask;
    }
}