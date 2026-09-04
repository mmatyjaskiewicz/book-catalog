namespace Application.DTOs.Requests;

public class CreateBookRequest
{
    public string Title { get; set; } = string.Empty;
    public Guid AuthorId { get; set; }
    public int PublishYear { get; set; }
}