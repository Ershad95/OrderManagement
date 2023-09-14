namespace Application.Events;

public record OrderDeleted(
    int OrderId,
    int ProductId,
    int UserId);