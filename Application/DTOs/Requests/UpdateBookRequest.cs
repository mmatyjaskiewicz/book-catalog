namespace Application.DTOs.Requests;

public class UpdateBookRequest
{
    public string? Title { get; set; }
    public string? Author { get; set; }
    public int? Year { get; set; }
}