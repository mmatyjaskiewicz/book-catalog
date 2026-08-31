using WebApi.Extensions;

namespace WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        DotNetEnv.Env.Load();
        
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddApplicationModules(builder.Configuration);
        
        var app = builder.Build();
        
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }
        
        app.UseExceptionHandler();
        
        app.UseHttpsRedirection();

        app.UseAuthorization();
        
        app.UseSwagger();
        
        app.UseSwaggerUI();
        
        app.MapControllers();
        
        app.Run();
    }
}