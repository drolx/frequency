using MQTTnet.AspNetCore;
using MQTTnet.Server;
using Proton.Frequency.Queue;
using Proton.Frequency.Services.ConfigOptions;
using System.Net;

namespace Proton.Frequency.Services;

internal static class MqttInitializer
{
    internal static WebApplicationBuilder RegisterMqttHost(this WebApplicationBuilder builder)
    {
        var config = builder.Configuration;
        var configOptions = new MqttOptions();
        var serverOptions = new ServerOptions();

        config.GetSection(MqttOptions.SectionKey).Bind(configOptions);
        config.GetSection(ServerOptions.SectionKey).Bind(serverOptions);
        
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
        var configOptions = new MqttOptions();
        config.GetSection(MqttOptions.SectionKey).Bind(configOptions);

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
