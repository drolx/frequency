using MQTTnet.AspNetCore;
using Proton.Frequency.Services.ConfigOptions;
using Serilog;
using System.Net;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Proton.Frequency.Services;

internal static class Initializer
{
    private const string Protocol = "http://";

    internal static ILogger GetLogger<T>()
    {
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.ClearProviders();
            builder.AddSerilog();
        });

        return loggerFactory.CreateLogger<T>();
    }

    private static IConfigurationBuilder LoadConfigurations(this IConfigurationBuilder configs)
    {
        configs.AddYamlFile("config.yaml", optional: false, reloadOnChange: true);
        var logger = GetLogger<IConfigurationBuilder>();
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
        builder.Services
            .AddOptions<MqttOptions>()
            .Bind(config.GetSection(MqttOptions.SectionKey))
            .ValidateDataAnnotations();

        return builder;
    }

    internal static WebApplicationBuilder RegisterHostOptions(this WebApplicationBuilder builder)
    {
        var config = builder.Configuration;
        var logger = GetLogger<WebApplicationBuilder>();
        var defaultOptions = new DefaultOptions();
        var serverOptions = new ServerOptions();
        var nodeOptions = new NodeOptions();
        var mqttOptions = new MqttOptions();

        config.GetSection(DefaultOptions.SectionKey).Bind(defaultOptions);
        config.GetSection(ServerOptions.SectionKey).Bind(serverOptions);
        config.GetSection(NodeOptions.SectionKey).Bind(nodeOptions);
        config.GetSection(MqttOptions.SectionKey).Bind(mqttOptions);

        var proxy = defaultOptions!.Proxy;
        var port = proxy ? nodeOptions.Port : serverOptions.Port;
        var host = proxy ? nodeOptions.Host : serverOptions.Host;

        logger.LogInformation("Setting up host options..");
        builder.WebHost.UseKestrel(o =>
        {
            o.Limits.MaxConcurrentConnections = 1024;
            o.Limits.MaxConcurrentUpgradedConnections = 1024;
            o.Limits.MaxRequestBodySize = 52428800;
            o.Listen( defaultOptions.Management ? host : IPAddress.None, port);
        });
        builder.RegisterMqttHost();
        builder.Host.ConfigureHostOptions(o => o.ShutdownTimeout = TimeSpan.FromSeconds(30));

        return builder;
    }
}
