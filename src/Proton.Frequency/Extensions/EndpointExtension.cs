using Proton.Frequency.Api;
using Proton.Frequency.Config;

namespace Proton.Frequency.Extensions;

internal static class EndpointExtension {
    internal static WebApplication RegisterEndpoints(this WebApplication app) {
        var logger = Initializer.GetLogger<WebApplication>();
        var defaultOptions = new ServiceConfig();
        app.Configuration.GetSection(ServiceConfig.Key).Bind(defaultOptions);
        app.RegisterQueueEndpoints();

        if (!defaultOptions.Api) {
            logger.LogInformation("API endpoints are disabled...");
            return app;
        }
        logger.LogInformation("Starting API endpoints...");
        app.MapControllers();
        app.RegisterApiEndpoints();

        if (!app.Environment.IsDevelopment())
            return app;
        app.UseSwagger();
        app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", $"{defaultOptions.Name}"); });

        return app;
    }
}
