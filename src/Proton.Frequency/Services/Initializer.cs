using Proton.Frequency.Services.ConfigOptions;
using Serilog;
using System.Diagnostics;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Proton.Frequency.Services;

internal static class Initializer
{
    private const string Protocol = "http://";

    private static DefaultOptions? DefaultConfigOptions { get; set; }
    
    private static ServerOptions? ServerConfigOptions { get; set; }

    private static NodeOptions? NodeConfigOptions { get; set; }

    internal static ILogger GetLogger()
    {
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        return loggerFactory.AddSerilog().CreateLogger<Program>();
    }

    private static IConfigurationBuilder LoadConfigurations(this IConfigurationBuilder configs)
    {
        configs.AddYamlFile("config.yaml", optional: false, reloadOnChange: true);
        var logger = GetLogger();
        var files = new List<string> { "node", "server", "network" };
        files.ForEach(n =>
        {
            var file = $"config.{n}.yaml";
            try
            {
                configs.AddYamlFile(file, optional: true, reloadOnChange: true);
            }
            catch (Exception e)
            {
                logger.LogError("Configuration contains error: {error}", e);
                throw;
            }
        });

        return configs;
    }

    private static WebApplicationBuilder SetupLogger(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration().WriteTo
            .Console()
            .WriteTo.File("logs/app.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog();
        builder.Host.UseSerilog();

        return builder;
    }

    internal static WebApplicationBuilder RegisterConfigurations(this WebApplicationBuilder builder)
    {
        builder.Host.UseSystemd();
        builder.SetupLogger();
        builder.Configuration.LoadConfigurations();
        
        var config = builder.Configuration;
        /* Minimal API configuration options */
        /* https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-6.0 */

        builder.Services.Configure<DefaultOptions>(config.GetSection(DefaultOptions.SectionKey));
        builder.Services
            .AddOptions<ServerOptions>()
            .Bind(config.GetSection(ServerOptions.SectionKey))
            .ValidateDataAnnotations();
        builder.Services
            .AddOptions<NodeOptions>()
            .Bind(config.GetSection(NodeOptions.SectionKey))
            .ValidateDataAnnotations();

        return builder;
    }

    internal static WebApplicationBuilder RegisterHostOptions(this WebApplicationBuilder builder)
    {
        var config = builder.Configuration;
        var logger = GetLogger();

        DefaultConfigOptions = config.GetSection(DefaultOptions.SectionKey).Get<DefaultOptions>();
        ServerConfigOptions = config.GetSection(ServerOptions.SectionKey).Get<ServerOptions>();
        NodeConfigOptions = config.GetSection(NodeOptions.SectionKey).Get<NodeOptions>();

        var proxy = DefaultConfigOptions!.Proxy;
        var url =
            Protocol
            + (
                proxy
                    ? $"{NodeConfigOptions!.Host}:{NodeConfigOptions!.Port}"
                    : $"{ServerConfigOptions!.Host}:{ServerConfigOptions!.Port}"
            );
        logger.LogInformation("Setting host options..");
        builder.WebHost.UseUrls(url);

        return builder;
    }
}
