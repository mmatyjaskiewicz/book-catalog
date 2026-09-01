namespace Application.DTOs.Requests;

public class UpdateBookRequest
{
    public string? Title { get; set; }
    public Guid? AuthorId { get; set; }
    public int? PublishYear { get; set; }
}