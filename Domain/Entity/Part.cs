namespace Domain.Entity;

public class Part : BaseEntity
{
    public string Name { get; private set; }

    public virtual List<User>? Users { get; private set; }

    public Part(string name)
    {
        Name = name;
        Users = new List<User>();
    }

    protected Part()
    {
    }
}