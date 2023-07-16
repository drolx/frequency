using Frequency.Common.Config;
using Frequency.Resources;

namespace Frequency.Extensions;

internal static class EndpointExtension {
    internal static WebApplication RegisterEndpoints(this WebApplication app) {
        var logger = Initializer.GetLogger<WebApplication>();
        var serviceOptions = new SystemConfig();
        var serverOptions = new ServerConfig();

        app.Configuration.GetSection(SystemConfig.Key).Bind(serviceOptions);
        app.Configuration.GetSection(ServerConfig.Key).Bind(serverOptions);
        app.RegisterQueueEndpoints();

        if (!serviceOptions.Api) {
            logger.LogInformation("API endpoints are disabled...");
            return app;
        }

        logger.LogInformation("Starting API endpoints...");
        app.MapControllers();
        app.RegisterApiEndpoints();

        if (!app.Environment.IsDevelopment())
            return app;
        app.UseSwagger();
        app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", $"{serverOptions.Name}"); });

        return app;
    }
}