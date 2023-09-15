using Application.Services;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Infrastructure.EventConsumer;

public class OrderCreated : IConsumer<Application.Events.OrderCreated>
{
    private readonly IEmailService _emailService;
    private readonly ISmsService _smsService;
    private readonly ILogger<OrderCreated> _logger;

    public OrderCreated(IEmailService emailService,
        ISmsService smsService,
        ILogger<OrderCreated> logger)
    {
        _emailService = emailService;
        _smsService = smsService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<Application.Events.OrderCreated> context)
    {
        await _emailService.SendAsync(context.Message.Email, context.CancellationToken);
        await _smsService.SendAsync(context.Message.MobileNumber, context.CancellationToken);
        _logger.LogInformation("notification sent to user");
    }
}
