using Proton.Frequency.Services.ConfigOptions;

namespace Proton.Frequency.Services;

internal static class MapEndpoints
{
    private static DefaultOptions? DefaultConfigOptions { get; set; }
    
    internal static WebApplication RegisterEndpoints(this WebApplication app)
    {
        var logger = Initializer.GetLogger();
        DefaultConfigOptions = app.Configuration.GetSection(DefaultOptions.SectionKey).Get<DefaultOptions>();
        var enabled = DefaultConfigOptions!.Api;
        
        if (!enabled) {
            logger.LogInformation("API endpoints are disabled...");
            return app;
        }
        logger.LogInformation("Starting API endpoints...");
        app.MapGet("/api/sample", () => "Hello");
        app.MapGet(
            "/api/sample/{name}",
            (string name) =>
            {
                app.Logger.LogInformation("Sample endpoint with params {name}", name);
                return $"Hello {name}";
            }
        );

        return app;
    }
}
