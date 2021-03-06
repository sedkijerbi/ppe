using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using server.Data;

namespace server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var dbContext = services.GetRequiredService<EmployeeDbContext>();
                    dbContext.Database.Migrate();
                    DataSeeder.SeedData(dbContext);
                }
                catch (Exception ex)
                {
                   var logger = services.GetRequiredService<ILogger<Program>>();
                   logger.LogError(ex, $"An error occurred during migration! Full error here: {ex}");
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
