namespace Application.DTOs.Responses;

public class BookResponse
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public int PublishYear { get; init; }
    public Guid AuthorId { get; init; }
}