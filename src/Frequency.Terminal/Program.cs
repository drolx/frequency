using System;
using System.Net.NetworkInformation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Proton.Frequency.Terminal.Data;
using Proton.Frequency.Terminal.Helpers;
using Proton.Frequency.Terminal.Handlers;
using Proton.Frequency.Terminal.Handlers.ReaderConnections;
using Proton.Frequency.Terminal.Protocols.Readers;

namespace Proton.Frequency.Terminal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(Figgle.FiggleFonts.Standard.Render("Proton Frequency"));
            var host = CreateHostBuilder(args).Build();
            ILogger logger = host.Services.GetService<ILogger<Program>>();

            logger.LogInformation("Starting Proton Frequency...");
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseWindowsService()
            .UseSystemd()
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
            })
            .ConfigureServices((hostContext, services) =>
            {
                /** Load Dependencies into DI container */
                services.AddSingleton<ConfigKey>();
                services.AddScoped<PingOptions>();
                services.AddScoped<Ping>();
                services.AddScoped<HTTPInitializer>();
                services.AddSingleton<SerialConnection>();
                services.AddTransient<ByteAssist>();
                services.AddTransient<SessionUtil>();
                services.AddScoped<FilterHandler>();
                services.AddScoped<NetworkCheck>();

                /** DB Connection required **/
                services.AddDbContext<CaptureContext>();
                services.AddScoped<CapturePersist>();
                services.AddScoped<PersistRequest>();
                services.AddSingleton<WebSync>();

                /** Extra DI Registration **/
                services.AddSingleton<KingJoinProtocol>();
                services.AddSingleton<ReaderProcess>();

                /** Worker service Registration **/
                services.AddHostedService<PipelineWorker>();
                services.AddHostedService<ForwardWorker>();

            })
            .UseDefaultServiceProvider(options =>
                    options.ValidateScopes = false)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config
                .AddYamlFile("defaults.yaml", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            });
    }
}
