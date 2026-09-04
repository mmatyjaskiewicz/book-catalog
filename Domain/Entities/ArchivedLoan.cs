namespace Domain.Entities;

public class ArchivedLoan : EntityBase
{
    public Guid BookId { get; private set; }
    public Book Book { get; set; } = null!;
    
    public Guid UserId { get; private set; }
    public User User { get; set; } = null!;
    
    public DateTime BorrowedAt { get; private set; }
    public DateTime ReturnedAt { get; private set; }

    public ArchivedLoan(Guid bookId, Guid userId, DateTime borrowedAt, DateTime returnedAt)
    {
        BookId = bookId;
        UserId = userId;
        BorrowedAt = borrowedAt;
        ReturnedAt = returnedAt;
    }
}