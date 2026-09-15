using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync(BookCatalogDbContext context)
    {
        await context.Database.MigrateAsync();
    }
}