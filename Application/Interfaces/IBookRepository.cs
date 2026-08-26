using Application.DTOs.Queries;
using Application.Models;
using Domain.Entities;

namespace Application.Interfaces;

public interface IBookRepository
{
    public Task AddAsync(Book book);
    public Task<PagedResult<Book>> GetAllAsync(BookQueryParameters queryParameters);
    public Task<Book?> GetByIdAsync(Guid id);
    public Task DeleteAsync(Book book);
    public Task UpdateAsync(Book book);
}