using Domain.Exceptions;

namespace Domain.Entities;

public class Book
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Title { get; private set; }
    public string Author { get; private set; }
    public int Year { get; private set; }
    
    public Book(string title, string author, int year)
    {
        ValidateTitle(title);
        ValidateAuthor(author);
        ValidateYear(year);
        
        Title = title;
        Author = author;
        Year = year;
    }
    
    public void UpdateTitle(string title)
    {
        ValidateTitle(title);
        Title = title;
    }
    
    public void UpdateAuthor(string author)
    { 
        ValidateAuthor(author);
        Author = author;
    }
    
    public void UpdateYear(int year)
    {
        ValidateYear(year);
        Year = year;
    }
    
    private static void ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Title is required.");

        if (title.Length > 200)
            throw new DomainException("Title cannot exceed 200 characters.");
    }
    
    private static void ValidateAuthor(string author)
    {
        if (string.IsNullOrWhiteSpace(author))
            throw new DomainException("Author is required.");

        if (author.Length > 200)
            throw new DomainException("Author cannot exceed 200 characters.");
    }
    
    private static void ValidateYear(int year)
    {
        if (year < 1 || year > DateTime.UtcNow.Year)
            throw new DomainException("Invalid publication year.");
    }
}