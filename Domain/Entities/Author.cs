namespace Domain.Entities;

public class Author : EntityBase
{
    public string Name { get; private set; }

    public ICollection<Book> Books { get; private set; } = [];
    
    public Author(string name)
    {
        Name = name;
    }
    
    // TODO: Add author update method
}