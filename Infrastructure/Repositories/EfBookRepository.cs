using Application.DTOs.Queries;
using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class EfBookRepository(BookCatalogDbContext context) : IBookRepository
{
    public Task AddAsync(Book book)
    {
        context.Books.Add(book);
        return context.SaveChangesAsync();
    }
    
    public Task<PagedResult<Book>> GetAllAsync(BookQueryParameters queryParameters)
    {
        throw new NotImplementedException();
    }
    
    public Task<Book?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
    
    public Task DeleteAsync(Book book)
    {
        context.Books.Remove(book);
        return context.SaveChangesAsync();
    }
    
    public Task UpdateAsync(Book book)
    {
        context.Books.Update(book);
        return context.SaveChangesAsync();
    }
}