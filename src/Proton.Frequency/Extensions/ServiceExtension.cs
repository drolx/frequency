using Microsoft.OpenApi.Models;
using Proton.Frequency.Api;
using Proton.Frequency.Config;

namespace Proton.Frequency.Extensions;

internal static class ServiceExtension
{
    internal static WebApplicationBuilder RegisterStandardServices(
        this WebApplicationBuilder builder
    )
    {
        var defaultOptions = new DefaultConfig();
        builder.Configuration.GetSection(DefaultConfig.Key).Bind(defaultOptions);
#if DEBUG
        builder.Services.AddSassCompiler();
#endif
        switch (defaultOptions.Management)
        {
            case false when defaultOptions.Api:
                return builder;
            case true:
                builder.Services.AddRazorPages();
                builder.Services.AddControllersWithViews();
                break;
        }

        if (!defaultOptions.Api) return builder;
        builder.Services.AddControllers();
        builder.Services.RegisterModules();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(
            "v1",
            new OpenApiInfo { Title = $"{defaultOptions.Name}", Version = "v1" }
            );
        });

        return builder;
    }

    internal static WebApplication RegisterDefaults(this WebApplication app)
    {
        var logger = Initializer.GetLogger<WebApplication>();
        var defaultOptions = new DefaultConfig();
        app.Configuration.GetSection(DefaultConfig.Key).Bind(defaultOptions);

        switch (defaultOptions.Management)
        {
            case false when !defaultOptions.Api:
                return app;
            case false:
                logger.LogInformation("Web management is disabled...");
                return app;
        }
        app.UseRouting();
        app.UseHttpsRedirection();
        app.UseAuthorization();

        if (defaultOptions.Api)
            app.RegisterEndpoints();

        logger.LogInformation("Starting web management UI...");
        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();
        app.MapRazorPages();
        app.MapControllers();
        app.MapFallbackToFile("index.html");
        
        if (!app.Environment.IsDevelopment())
            return app;
        app.UseWebAssemblyDebugging();
        app.UseExceptionHandler("/Error");

        return app;
    }
}
