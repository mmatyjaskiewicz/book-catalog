using Domain.Entities;

namespace Application.Interfaces;

public interface IBookRepository
{
    public Task AddAsync(Book book);
    public Task<List<Book>> GetAllAsync();
    public Task<Book?> GetByIdAsync(Guid id);
    public Task DeleteAsync(Book book);
}