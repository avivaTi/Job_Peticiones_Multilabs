using Aviva.Multilabs.Peticiones.Demonio.services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aviva.Multilabs.Peticiones.Demonio
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();
            Log.Logger = new LoggerConfiguration()
                //.MinimumLevel.ControlledBy(levelSwitch)
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.File(@"D:\log\Multilabs\logMultilabs.txt")
                .CreateLogger();

            try
            {
                Log.Information("Iniciando Demonio de Presupuestos");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal($"Error al iniciar Demonio de Presupuestos - {ex.Message}");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.AddScoped<ILoggerService, LoggerService>();
                }).UseSerilog()
                .UseWindowsService()
            ;
    }
}
