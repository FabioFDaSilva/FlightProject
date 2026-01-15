using System;
using XmlAPI.Services;
using XmlAPI.Models;

namespace XmlAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                     policy.WithOrigins("http://localhost:3000")
                           .AllowAnyHeader()
                           .AllowAnyMethod());
            });

            builder.Services.AddControllers();
            builder.Services.AddScoped<FlightDataReader>();
            builder.Services.AddScoped<FlightPrinter>();

            var app = builder.Build();
            app.UseCors(); // enable the CORS policy
            app.UseAuthorization();
            
            app.MapControllers();
            
            app.Run();
            
            using (var scope = app.Services.CreateScope())
            {
                var printer = scope.ServiceProvider.GetRequiredService<FlightPrinter>();
                printer.PrintFlights(); // prints all flights to console
            }
            // Map controller routes
            app.MapControllers();

            // Start the server
            app.Run();
        }
    }
}

