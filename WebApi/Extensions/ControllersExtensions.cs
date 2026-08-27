using WebApi.Filters;

namespace WebApi.Extensions;

public static class ControllersExtensions
{
    public static IServiceCollection AddControllersConfiguration(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add<ValidationFilter>();
        });

        services.AddAuthorization();

        return services;
    }
}