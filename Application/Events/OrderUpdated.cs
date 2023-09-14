namespace Application.Events;

public record OrderUpdated(
    int OrderId,
    int ProductId,
    int UserId);