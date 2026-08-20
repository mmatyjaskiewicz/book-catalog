using Application.Interfaces;
using Infrastructure.Repositories;

namespace WebApi.Extensions;

public static class PersistenceExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddScoped<IBookRepository, FakeBookRepository>();

        return services;
    }
}