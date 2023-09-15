namespace Domain.Entity;

public class Part : BaseEntity
{
    public string Name { get; private set; }

    public Part(string name)
    {
        Name = name;
    }

    protected Part()
    {
        
    }
}