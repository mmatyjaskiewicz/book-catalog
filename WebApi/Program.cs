using Infrastructure.Persistence;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using WebApi.Extensions;

namespace WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        DotNetEnv.Env.TraversePath().Load();
        
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddApplicationModules(builder.Configuration);
        builder.Services
            .AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy(), tags: new[] { "live" })
            .AddDbContextCheck<BookCatalogDbContext>(tags: new[] { "ready" });
        
        var app = builder.Build();
        
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