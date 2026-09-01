namespace Application.DTOs.Queries;

public class BookQueryParameters
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    
    public string? Title { get; set; } = string.Empty;
    public Guid? AuthorId { get; set; }
    public int? PublishYear { get; set; } = null;
}