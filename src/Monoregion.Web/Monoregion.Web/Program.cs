namespace Monoregion.Web;

using Microsoft.AspNetCore.Datasync;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public class Program
{
    public static void Main(string[] args)
    {

        var app = GetBuildedApplication(args);

        ConfigureMiddlewares(app);

        app.Run();
    }

    private static void ConfigureMiddlewares(WebApplication app)
    {
        app.UseHttpsRedirection();
        app.UseHsts();

        app.UseRouting();

        app.UseAuthorization();

        app.MapDefaultControllerRoute();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }

    private static WebApplication GetBuildedApplication(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthorization();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // TODO: use other db provider
        builder.Services.AddDbContext<ApplicationDbContext>(o => o.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddControllers();
        builder.Services.AddDatasyncControllers();

        return builder.Build();
    }
}