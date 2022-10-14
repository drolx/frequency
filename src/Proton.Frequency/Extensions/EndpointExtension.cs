using Proton.Frequency.Config;
using Proton.Frequency.Module;

namespace Proton.Frequency.Extensions;

internal static class EndpointExtension
{
    internal static WebApplication RegisterEndpoints(this WebApplication app)
    {
        var logger = Initializer.GetLogger<WebApplication>();
        var serviceOptions = new ServiceConfig();
        var serverOptions = new ServerConfig();
        
        app.Configuration.GetSection(ServiceConfig.Key).Bind(serviceOptions);
        app.Configuration.GetSection(ServerConfig.Key).Bind(serverOptions);
        app.RegisterQueueEndpoints();

        if (!serviceOptions.Api)
        {
            logger.LogInformation("API endpoints are disabled...");
            return app;
        }
        logger.LogInformation("Starting API endpoints...");
        app.MapControllers();
        app.RegisterApiEndpoints();

        if (!app.Environment.IsDevelopment())
            return app;
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", $"{serverOptions.Name}");
        });

        return app;
    }
}
