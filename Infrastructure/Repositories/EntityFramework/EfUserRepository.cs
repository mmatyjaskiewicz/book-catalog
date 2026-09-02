using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories.EntityFramework;

public class EfUserRepository : EfRepository<User>, IUserRepository
{
    private readonly BookCatalogDbContext _context;
    
    public EfUserRepository(BookCatalogDbContext context) : base(context)
    {
        _context = context;
    }
}