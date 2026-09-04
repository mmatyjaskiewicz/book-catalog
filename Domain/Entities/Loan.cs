namespace Domain.Entities;

public class Loan : EntityBase
{
    public Guid BookId { get; private set; }
    public Book Book { get; set; } = null!;
    
    public Guid UserId { get; private set; }
    public User User { get; set; } = null!;
    
    public DateTime BorrowedAt { get; private set; }
    public DateTime? ReturnedAt { get; private set; }
    
    public Loan(Guid bookId, Guid userId)
    {
        BookId = bookId;
        UserId = userId;
        BorrowedAt = DateTime.UtcNow;
    }
    
    public void Return()
    {
        ReturnedAt = DateTime.UtcNow;
    }
}