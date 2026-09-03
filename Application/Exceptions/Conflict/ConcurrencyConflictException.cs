namespace Application.Exceptions.Conflict;

public class ConcurrencyConflictException(string message) : ConflictException(message) { }