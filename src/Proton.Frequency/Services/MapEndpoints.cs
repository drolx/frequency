using Proton.Frequency.Api;
using Proton.Frequency.Services.ConfigOptions;

namespace Proton.Frequency.Services;

internal static class MapEndpoints
{
    internal static WebApplication RegisterEndpoints(this WebApplication app)
    {
        var logger = Initializer.GetLogger();
        var defaultOptions = new DefaultOptions();
        app.Configuration.GetSection(DefaultOptions.SectionKey).Bind(defaultOptions);
        var enabled = defaultOptions.Api;

        if (!enabled)
        {
            logger.LogInformation("API endpoints are disabled...");
            return app;
        }
        logger.LogInformation("Starting API endpoints...");
        app.MapControllers();
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", $"{defaultOptions.Name} Api");
        });
        app.RegisterApiEndpoints();

        /* Basic endpoints */
        app.MapGet("/api/health", () => 200);

        return app;
    }
}
