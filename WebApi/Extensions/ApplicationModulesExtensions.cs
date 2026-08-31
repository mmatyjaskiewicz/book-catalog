namespace WebApi.Extensions;

public static class ApplicationModulesExtensions
{
    public static IServiceCollection AddApplicationModules(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllersConfiguration();
        services.AddPersistence(configuration);
        services.AddServices();
        services.AddValidation();
        services.AddExceptionHandling();
        services.AddSwagger();
        
        return services;
    }
}