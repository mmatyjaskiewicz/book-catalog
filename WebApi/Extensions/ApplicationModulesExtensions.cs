namespace WebApi.Extensions;

public static class ApplicationModulesExtensions
{
    public static IServiceCollection AddApplicationModules(this IServiceCollection services)
    {
        services.AddPersistence();
        services.AddServices();

        return services;
    }
}