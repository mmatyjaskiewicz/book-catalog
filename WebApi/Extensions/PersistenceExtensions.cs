using Application.Interfaces.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Repositories.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Extensions;

public static class PersistenceExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        services.AddDbContext<BookCatalogDbContext>(options => options.UseNpgsql(connectionString));
        
        services.AddScoped<IBookRepository, EfBookRepository>();
        
        // services.AddSingleton<IBookRepository, FakeBookRepository>();

        return services;
    }
}