using MQTTnet.AspNetCore;
using MQTTnet.Server;
using Proton.Frequency.Config;
using Proton.Frequency.Queue;
using System.Net;

namespace Proton.Frequency.Extensions;

internal static class QueueExtension
{
    internal static WebApplicationBuilder RegisterMqttHost(this WebApplicationBuilder builder)
    {
        var config = builder.Configuration;
        var configOptions = new QueueConfig();
        var serverOptions = new ServerConfig();

        config.GetSection(QueueConfig.Key).Bind(configOptions);
        config.GetSection(ServerConfig.Key).Bind(serverOptions);
        
        if (!configOptions.Enable) return builder;
        
        builder.Services
            .AddHostedMqttServer(options =>
            {
                options
                    .WithPersistentSessions()
                    .WithDefaultEndpoint()
                    .WithConnectionBacklog(100)
                    .WithDefaultEndpointPort(configOptions.Port);
            })
            .AddMqttConnectionHandler()
            .AddConnections();
        builder.WebHost.UseKestrel(o =>
        {
            o.Listen(serverOptions.Host, configOptions.Port, l => l.UseMqtt());
        });

        return builder;
    }

    internal static WebApplication RegisterMqttEndpoints(this WebApplication app) {
        var config = app.Configuration;
        var configOptions = new QueueConfig();
        config.GetSection(QueueConfig.Key).Bind(configOptions);

        if (!configOptions.Enable) return app;
        app.MapMqtt("/queue-server");
        app.UseMqttServer(server =>
        {
            server.ValidatingConnectionAsync += QueueController.ValidateConnection;
            server.ClientConnectedAsync += QueueController.OnClientConnected;
        });

        return app;
    }
}
