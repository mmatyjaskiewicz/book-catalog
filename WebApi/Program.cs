using Infrastructure.Persistence;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using WebApi.Extensions;

namespace WebApi;

public class Program
{
    public static async Task Main(string[] args)
    {
        DotNetEnv.Env.TraversePath().Load();
        
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddApplicationModules(builder.Configuration);
        
        var app = builder.Build();
        
        await using (var scope = app.Services.CreateAsyncScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<BookCatalogDbContext>();
            await DatabaseInitializer.InitializeAsync(dbContext);
        }
        
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }
        
        app.UseExceptionHandler();
        
        app.UseHttpsRedirection();

        app.UseAuthorization();
        
        app.MapHealthChecks("/health/live", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("live")
        });
        
        app.MapHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("ready")
        });
        
        app.UseSwagger();
        
        app.UseSwaggerUI();
        
        app.MapControllers();
        
        app.Run();
    }
}