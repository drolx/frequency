using Proton.Frequency.Config;
using Proton.Frequency.Extensions;
using Serilog;
using System.Net;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Proton.Frequency;

internal static class Initializer
{
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
        var files = new List<string> { "proxy", "server", "protocol" };
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
        builder.Services.AddOptions<ServiceConfig>().Bind(config.GetSection(ServiceConfig.Key));
        builder.Services.AddOptions<ServerConfig>().Bind(config.GetSection(ServerConfig.Key));
        builder.Services.AddOptions<ProxyConfig>().Bind(config.GetSection(ProxyConfig.Key));
        builder.Services.AddOptions<QueueConfig>().Bind(config.GetSection(QueueConfig.Key));
        builder.Services.AddOptions<DatabaseConfig>().Bind(config.GetSection(DatabaseConfig.Key));
        builder.Services.AddOptions<List<DeviceConfig>>().Bind(config.GetSection(DeviceConfig.Key));
        builder.Services
            .AddOptions<List<NetworkConfig>>()
            .Bind(config.GetSection(NetworkConfig.Key));

        return builder;
    }

    internal static WebApplicationBuilder RegisterHostOptions(this WebApplicationBuilder builder)
    {
        var config = builder.Configuration;
        var logger = GetLogger<WebApplicationBuilder>();
        var defaultOptions = new ServiceConfig();
        var serverOptions = new ServerConfig();
        var proxyOptions = new ProxyConfig();
        var queueOptions = new QueueConfig();

        config.GetSection(ServiceConfig.Key).Bind(defaultOptions);
        config.GetSection(ServerConfig.Key).Bind(serverOptions);
        config.GetSection(ProxyConfig.Key).Bind(proxyOptions);
        config.GetSection(QueueConfig.Key).Bind(queueOptions);

        var proxy = defaultOptions!.Proxy;
        var port = proxy ? proxyOptions.Port : serverOptions.Port;
        var host = proxy ? proxyOptions.Host : serverOptions.Host;

        logger.LogInformation("Setting up host options..");
        builder.WebHost.UseKestrel(o =>
        {
            o.Limits.MaxConcurrentConnections = 1024;
            o.Limits.MaxConcurrentUpgradedConnections = 1024;
            o.Limits.MaxRequestBodySize = 52428800;
            o.Listen(defaultOptions.Management ? host : IPAddress.None, port);
        });
        builder.RegisterQueueHost();
        builder.Host.ConfigureHostOptions(o => o.ShutdownTimeout = TimeSpan.FromSeconds(30));

        return builder;
    }
}
