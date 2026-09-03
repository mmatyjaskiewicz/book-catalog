using Application.Exceptions.Conflict;
using Application.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Infrastructure.Repositories;

public class EfRepository<T>(BookCatalogDbContext context) : IRepository<T> where T : class
{
    public async Task AddAsync(T entity)
    {
        context.Set<T>().Add(entity);
        
        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException { SqlState: "23505", ConstraintName: "ix_loans_book_id_active" })
        {
            throw new ConcurrencyConflictException("Book is already borrowed.");
        }
    }

    public Task UpdateAsync(T entity)
    {
        context.Set<T>().Update(entity);
        return context.SaveChangesAsync();
    }

    public Task DeleteAsync(T entity)
    {
        context.Set<T>().Remove(entity);
        return context.SaveChangesAsync();
    }

    public Task<List<T>> GetAllAsync()
    {
        return context.Set<T>().ToListAsync();
    }

    public Task<T?> GetByIdAsync(Guid id)
    {
        return context.Set<T>().FindAsync(id).AsTask();
    }
}