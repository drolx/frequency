using MQTTnet.AspNetCore;
using MQTTnet.Server;
using Proton.Frequency.Services.ConfigOptions;

namespace Proton.Frequency.Services;

internal static class MqttInstance
{
    internal static WebApplicationBuilder RegisterMqttHost(this WebApplicationBuilder builder) {
        var config = builder.Configuration;
        var configOptions = new MqttOptions();
        var serverOptions = new ServerOptions();
        
        config.GetSection(MqttOptions.SectionKey).Bind(configOptions);
        config.GetSection(ServerOptions.SectionKey).Bind(serverOptions);
        
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
        app.MapMqtt("/queue-server");
        app.UseMqttServer(
        server =>
        {
            server.ValidatingConnectionAsync += ValidateConnection;
            server.ClientConnectedAsync += OnClientConnected;
        });
        
        return app;
    }
    
    private static Task OnClientConnected(ClientConnectedEventArgs eventArgs)
    {
        var logger = Initializer.GetLogger<WebApplication>();
        logger.LogInformation("Client '{id}' connected.", eventArgs.ClientId);
        return Task.CompletedTask;
    }
    
    private static Task ValidateConnection(ValidatingConnectionEventArgs eventArgs)
    {
        var logger = Initializer.GetLogger<WebApplication>();
        logger.LogInformation("Client '{id}' ants to connect. Accepting!", eventArgs.ClientId);
        return Task.CompletedTask;
    }
}