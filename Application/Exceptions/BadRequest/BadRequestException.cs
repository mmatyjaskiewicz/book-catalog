namespace Application.Exceptions.BadRequest;

public class BadRequestException(string message) : AppException(message) { }