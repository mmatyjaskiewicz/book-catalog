using Domain.Exceptions;

namespace Domain.Entities;

public class Book : EntityBase
{ 
    public string Title { get; private set; }
    public int PublishYear { get; private set; }
    
    public Guid AuthorId { get; private set; }
    public Author Author { get; set; } = null!;
    
    public Book(string title, Guid authorId, int publishYear)
    {
        ValidateTitle(title);
        ValidatePublishYear(publishYear);

        Title = title;
        AuthorId = authorId;
        PublishYear = publishYear;
    }
    
    public void UpdateTitle(string title)
    {
        ValidateTitle(title);
        Title = title;
    }
    
    public void UpdatePublishYear(int publishYear)
    {
        ValidatePublishYear(publishYear);
        PublishYear = publishYear;
    }
    
    public void UpdateAuthorId(Guid authorId)
    {
        ValidateAuthorId(authorId);
        AuthorId = authorId;
    }
    
    private static void ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Title is required.");

        if (title.Length > 100)
            throw new DomainException("Title cannot exceed 100 characters.");
    }
    
    private static void ValidatePublishYear(int publishYear)
    {
        if (publishYear < 1 || publishYear > DateTime.UtcNow.Year)
            throw new DomainException("Invalid publication year.");
    }
    
    public static void ValidateAuthorId(Guid authorId)
    {
        if (authorId == Guid.Empty)
            throw new DomainException("AuthorId is required.");
    }
}