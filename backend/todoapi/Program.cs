using Microsoft.EntityFrameworkCore;
using todoapi.Data;
using todoapi.Interfaces;
using todoapi.Repositories;

namespace todoapi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Database connection
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<ITodoRepository, TodoRepository>();

            // HTTPS redirection
            builder.Services.AddHttpsRedirection(options =>
            {
                options.HttpsPort = 443;
            });

            // ✅ Add CORS BEFORE Build()
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    policy => policy.WithOrigins(
                        "http://127.0.0.1:5500",
                        "http://localhost:5500",
                        "https://cdc8db99.todoapp-7sx.pages.dev"
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod());
            });

            var app = builder.Build();

            // ✅ Auto migrate database
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate();
            }

            // ✅ Middleware order matters
            app.UseHttpsRedirection();
            app.UseCors("AllowFrontend");

            // ✅ Always enable Swagger (not just Development)
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API v1");
                c.RoutePrefix = "swagger"; // URL will be /swagger
            });

            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}