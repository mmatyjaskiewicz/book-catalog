namespace Domain.Entities;

public class Author : EntityBase
{
    public string Name { get; private set; }

    public ICollection<Book> Books { get; private set; } = [];
    
    public Author(string name)
    {
        ValidateName(name);
        Name = name;
    }
    
    public void Update(string name)
    {
        ValidateName(name);
        Name = name;
    }
    
    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Author name cannot be empty.");
        }
        
        if (name.Length > 100)
        {
            throw new ArgumentException("Author name cannot exceed 100 characters.");
        }
    }
}