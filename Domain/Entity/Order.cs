namespace Domain.Entity;

public class Order : BaseEntity
{
    public int UserId { get; private set; }
    public int ProductId { get; private set; }
    public int PartId { get; private set; }
    public DateTime CreatedDateTime { get; private set; }
    public bool Deleted { get; private set; }

    public virtual User User { get; private set; }
    public virtual Product Product { get; private set; }
    public virtual Part Part { get; private set; }

    protected Order()
    {
    }

    public Order(int userId, int productId, int partId, DateTime createdDateTime)
    {
        UserId = userId;
        ProductId = productId;
        PartId = partId;
        CreatedDateTime = createdDateTime;
    }

    public void MarkAsDeleted()
    {
        Deleted = true;
    }

    public void UpdateOrder(int productId, int partId)
    {
        ProductId = productId;
        PartId = partId;
    }
}