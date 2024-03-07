
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using proj2.Models;

namespace proj2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string txt = ""; 
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<HRContext>(options => { options.UseSqlServer(builder.Configuration.GetConnectionString("con")); });
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(txt,
                builder =>
                {

                    builder.AllowAnyOrigin();
                   // builder.WithOrigins();
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();

                app.UseSwaggerUI();

                try
                {
                    var dbContext = app.Services.CreateScope().ServiceProvider.GetRequiredService<HRContext>();
                    dbContext.Database.Migrate();
                }
                catch (Exception ex)
                {
                    // Handle the exception as needed
                    Console.WriteLine($"Error applying migrations: {ex.Message}");
                }
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseCors(txt);

            app.MapControllers();

            app.Run("http://0.0.0.0:5000");
        }
    }
}
