using Application.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EfRepository<T>(BookCatalogDbContext context) : IRepository<T> where T : class
{
    public Task AddAsync(T entity)
    {
        context.Set<T>().Add(entity);
        return context.SaveChangesAsync();
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