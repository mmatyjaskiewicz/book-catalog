using Application.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Extensions;

public static class PersistenceExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        services.AddDbContext<BookCatalogDbContext>(options =>
            options.UseNpgsql(connectionString));
        
        services.AddSingleton<IBookRepository, FakeBookRepository>();

        return services;
    }
}