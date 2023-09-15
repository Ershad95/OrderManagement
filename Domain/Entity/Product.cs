namespace Domain.Entity;

public class Product : BaseEntity
{
    public string Name { get; private set; }

    public Product(string name)
    {
        Name = name;
    }
}