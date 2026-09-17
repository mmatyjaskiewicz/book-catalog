using Infrastructure.Persistence;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using WebApi.Extensions;

namespace WebApi;

public class Program
{
    public static async Task Main(string[] args)
    {
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
        {
            DotNetEnv.Env.TraversePath().Load();
        }
        
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddApplicationModules(builder.Configuration);
        
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        
        try
        {
            var connectionStringBuilder = new Npgsql.NpgsqlConnectionStringBuilder(connectionString);

            Console.WriteLine(
                $"PostgreSQL config OK: Host={connectionStringBuilder.Host}, " +
                $"Port={connectionStringBuilder.Port}, " +
                $"Database={connectionStringBuilder.Database}, " +
                $"Username={connectionStringBuilder.Username}, " +
                $"SslMode={connectionStringBuilder.SslMode}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"PostgreSQL connection string ERROR: {ex.GetType().Name}: {ex.Message}");
        }
        
        var app = builder.Build();
        
        if(app.Environment.IsDevelopment())
        {
            await using (var scope = app.Services.CreateAsyncScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BookCatalogDbContext>();
                await DatabaseInitializer.InitializeAsync(dbContext);
            }
        }
        
        app.MapOpenApi();
        
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