using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace TradingApp.Web.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>

                    webBuilder.UseStartup<Startup>()
                        .UseKestrel(options => options.AddServerHeader = false)
                        .UseSerilog((Ctx, LoggerConfiguration) =>
                        {
                            //Console.WriteLine("ha pasado por aquii");
                            string date = DateTime.Now.ToShortDateString().Replace("/", "");

                            LoggerConfiguration.WriteTo.File($"/logs/{date} _log.txt");
                        })
                );
                

    }
}
