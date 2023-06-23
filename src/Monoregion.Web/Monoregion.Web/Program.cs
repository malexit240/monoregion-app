namespace Monoregion.Web;

using Microsoft.AspNetCore.Datasync;
using Microsoft.EntityFrameworkCore;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthorization();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<ApplicationDbContext>(o => o.UseSqlite("Filename=monoregion.db"));

        builder.Services.AddControllers();
        builder.Services.AddDatasyncControllers();

        var app = builder.Build();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
        });

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.Run();
    }
}