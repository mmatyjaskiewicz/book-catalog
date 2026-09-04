namespace Application.Interfaces;

public interface IRepository<T> where T : class
{
    public Task AddAsync(T entity);
    public Task UpdateAsync(T entity);
    public Task DeleteAsync(T entity);
    public Task<T?> GetByIdAsync(Guid id);
}