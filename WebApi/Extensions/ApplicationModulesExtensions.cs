namespace WebApi.Extensions;

public static class ApplicationModulesExtensions
{
    public static IServiceCollection AddApplicationModules(this IServiceCollection services)
    {
        services.AddControllersConfiguration();
        services.AddPersistence();
        services.AddServices();
        services.AddValidation();
        services.AddExceptionHandling();
        services.AddSwagger();
        
        return services;
    }
}