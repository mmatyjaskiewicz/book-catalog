namespace Application.DTOs.Requests;

public class UpdateUserRequest
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
}