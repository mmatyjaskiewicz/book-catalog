using Application.DTOs.Queries;
using Application.Models;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IAuthorRepository : IRepository<Author>
{
    public Task<PagedResult<Author>> GetAllAsync(AuthorQueryParameters queryParameters);
}