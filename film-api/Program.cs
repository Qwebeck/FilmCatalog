using FilmApi.DAL;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using FIlmApi.DAL;

namespace FilmApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Debug.WriteLine("First message");
            var host = CreateHostBuilder(args).Build();
            CreateDbIfNotExists(host);
            host.Run();
            Debug.WriteLine("Here");
        }
        private static void CreateDbIfNotExists(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                Debug.WriteLine("Everything is starting");
                try
                {
                    var context = services.GetRequiredService<Context>();
                    DBInitializer.Initialize(context);
                    Debug.WriteLine("All is cool");
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                    Debug.WriteLine("Exception");
                }
            }
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
