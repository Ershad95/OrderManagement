using MassTransit;

namespace Infrastructure.EventConsumer;

public class OrderDeleted : IConsumer<Application.Events.OrderDeleted>
{
    public Task Consume(ConsumeContext<Application.Events.OrderDeleted> context)
    {
        return Task.CompletedTask;
    }
}