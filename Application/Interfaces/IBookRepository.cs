using Domain.Entities;

namespace Application.Interfaces;

public interface IBookRepository
{
    Task AddAsync(Book book);
    Task<List<Book>> GetAllAsync();
    Task<Book?> GetByIdAsync(Guid id);
    Task DeleteAsync(Guid id);
}