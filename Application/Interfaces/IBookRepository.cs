using Application.DTOs.Queries;
using Application.Models;
using Domain.Entities;

namespace Application.Interfaces;

public interface IBookRepository : IRepository<Book>
{
    public Task<PagedResult<Book>> GetAllAsync(BookQueryParameters queryParameters);
}