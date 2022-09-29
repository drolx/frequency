using Proton.Frequency.Api;
using Proton.Frequency.Services.ConfigOptions;

namespace Proton.Frequency.Services;

internal static class MapEndpoints
{
    internal static WebApplication RegisterEndpoints(this WebApplication app)
    {
        var logger = Initializer.GetLogger<WebApplication>();
        var defaultOptions = new DefaultOptions();
        app.Configuration.GetSection(DefaultOptions.SectionKey).Bind(defaultOptions);
        var enabled = defaultOptions.Api;
        app.RegisterMqttEndpoints();

        if (!enabled)
        {
            logger.LogInformation("API endpoints are disabled...");
            return app;
        }
        logger.LogInformation("Starting API endpoints...");
        app.MapControllers();
        app.RegisterApiEndpoints();

        /* Basic endpoints */
        // app.MapGet("/api/health", () => 200);

        if (!app.Environment.IsDevelopment())
            return app;
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", $"{defaultOptions.Name} Api");
        });

        return app;
    }
}
