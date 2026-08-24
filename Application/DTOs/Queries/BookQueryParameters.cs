namespace Application.DTOs.Queries;

public class BookQueryParameters
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}