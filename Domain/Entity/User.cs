namespace Domain.Entity;

public class User
{
    public int Id { get; private set; }
    public Guid Guid { get; private set; }
    public string UserName { get; private set; }
    public string Password { get; private set; }
    public string MobileNumber { get; private set; }
    public string Email { get; private set; }

    public User(string userName, string password, string mobileNumber, string email)
    {
        UserName = userName;
        Password = password;
        MobileNumber = mobileNumber;
        Email = email;
        Guid = Guid.NewGuid();
    }
}