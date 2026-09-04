using Application.Services;

namespace WebApi.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<BookService>();
        services.AddScoped<AuthorService>();
        services.AddScoped<LoanService>();
        
        return services;
    }
}