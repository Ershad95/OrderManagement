namespace Application.Events;

public record OrderCreated(
    int OrderId,
    int ProductId,
    int UserId,
    string MobileNumber,
    string Email);