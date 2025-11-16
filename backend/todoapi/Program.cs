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

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<ITodoRepository, TodoRepository>();

            // ✅ Add CORS BEFORE Build()
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    policy => policy.WithOrigins("http://127.0.0.1:5500/", "http://localhost:5500", "http://127.0.0.1:5500") // your frontend origin
                                    .AllowAnyHeader()
                                    .AllowAnyMethod());
            });

            var app = builder.Build();

            // ✅ Use CORS AFTER Build()
            app.UseHttpsRedirection();
            app.UseCors("AllowFrontend");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}