using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DotNetWMS
{
    /// <summary>
    /// Main class responsible for starting the application
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main point of application
        /// </summary>
        /// <param name="args">Command-line arguments</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        /// <summary>
        /// Static method responsible for the host start configuration
        /// </summary>
        /// <param name="args">Command-line arguments</param>
        /// <returns>Returns the configured object of the Host class</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                 .ConfigureLogging((hostingContext, logging) =>
                 {
                     logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                     logging.AddConsole();
                     logging.AddDebug();
                     logging.AddEventSourceLogger();
                 })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
