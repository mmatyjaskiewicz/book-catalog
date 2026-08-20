using Application.Interfaces;
using Infrastructure.Repositories;

namespace WebApi.Extensions;

public static class PersistenceExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddSingleton<IBookRepository, FakeBookRepository>();

        return services;
    }
}