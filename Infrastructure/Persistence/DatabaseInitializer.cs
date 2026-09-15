using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync(BookCatalogDbContext context)
    {
        await context.Database.MigrateAsync();
        await SeedAsync(context);
    }
    
    private static async Task SeedAsync(BookCatalogDbContext dbContext)
    {
        if (await dbContext.Authors.AnyAsync())
        {
            return;
        }

        var authors = new[]
        {
            new Author("George Orwell"),
            new Author("J.R.R. Tolkien"),
            new Author("Frank Herbert")
        };

        dbContext.Authors.AddRange(authors);

        var users = new[]
        {
            new User("user"),
            new User("user_mock"),
            new User("user_test")
        };

        dbContext.Users.AddRange(users);

        var books = new[]
        {
            new Book("1984", authors[0].Id, 1949),
            new Book("The Hobbit", authors[1].Id, 1937),
            new Book("Dune", authors[2].Id, 1965)
        };

        dbContext.Books.AddRange(books);

        await dbContext.SaveChangesAsync();
    }
}