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

            // Register controllers
            builder.Services.AddControllers();

            // Register FlightDataReader so controller can inject it
            builder.Services.AddScoped<FlightDataReader>();
            builder.Services.AddScoped<FlightPrinter>();


            var app = builder.Build();
            
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

