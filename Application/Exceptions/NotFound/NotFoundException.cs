namespace Application.Exceptions.NotFound;

public class NotFoundException(string message) : AppException(message) { }