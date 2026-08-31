using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class BookCatalogDbContext(DbContextOptions<BookCatalogDbContext> options) : DbContext(options)
{
    public DbSet<Book> Books => Set<Book>();
}