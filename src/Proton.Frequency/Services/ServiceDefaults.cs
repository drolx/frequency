using Proton.Frequency.Services.ConfigOptions;

namespace Proton.Frequency.Services;

internal static class ServiceDefaults
{
    private static DefaultOptions? DefaultConfigOptions { get; set; }
    
    internal static IServiceCollection RegisterStandardServices(this IServiceCollection services)
    {
        services.AddRazorPages();
#if DEBUG
        services.AddSassCompiler();
#endif
        return services;
    }

    internal static WebApplication RegisterDefaults(this WebApplication app) {
        var logger = Initializer.GetLogger();
        DefaultConfigOptions = app.Configuration.GetSection(DefaultOptions.SectionKey).Get<DefaultOptions>();
        var enabled = DefaultConfigOptions!.Management;

        if (!enabled) {
            logger.LogInformation("Management is disabled...");
            return app;
        }
        
        logger.LogInformation("Starting management UI...");
        app.MapRazorPages();
        app.UseHttpsRedirection().UseStaticFiles().UseRouting().UseAuthorization();

        if (!app.Environment.IsDevelopment()) return app;
        app.UseExceptionHandler("/Error");
        app.UseHsts();

        return app;
    }
}
