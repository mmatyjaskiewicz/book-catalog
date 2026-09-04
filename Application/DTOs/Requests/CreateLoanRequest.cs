namespace Application.DTOs.Requests;

public class CreateLoanRequest
{
    public Guid BookId { get; set; }
    public Guid UserId { get; set; }
}