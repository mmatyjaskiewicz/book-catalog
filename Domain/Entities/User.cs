namespace Domain.Entities;

public class User : EntityBase
{
    public string Username { get; private set; }

    public ICollection<Loan> Loans { get; private set; } = [];
    
    public User(string username)
    {
        Username = username;
    }
}