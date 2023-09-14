namespace Domain.Entity;

public class User
{
    public int Id { get; private set; }
    public Guid Guid { get;  set; }
    public string UserName { get;  set; }
    public string Password { get;  set; }
    public string MobileNumber { get; private set; }
    public string Email { get; private set; }
}