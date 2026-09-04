namespace Application.DTOs.Queries;

public class LoanQueryParameters
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    
    public Guid? UserId { get; set; }
    public Guid? BookId { get; set; }
}