
using AzureWebApp.API.Context;
using Microsoft.EntityFrameworkCore;

namespace AzureWebApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("ProdConnection")));

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapGet("/", () => "Welcome My API Home Page!");
            app.UseAuthorization();

            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var srv = scope.ServiceProvider;
                var context = srv.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();
            }

            app.Run();
        }
    }
}
