using Microsoft.OpenApi.Models;
using Proton.Frequency.Api;
using Proton.Frequency.Services.ConfigOptions;

namespace Proton.Frequency.Services;

internal static class ServiceDefaults
{
    internal static WebApplicationBuilder RegisterStandardServices(
        this WebApplicationBuilder builder
    )
    {
#if DEBUG
        builder.Services.AddSassCompiler();
#endif
        var defaultOptions = new DefaultOptions();
        builder.Configuration.GetSection(DefaultOptions.SectionKey).Bind(defaultOptions);
        if (defaultOptions.Management)
            builder.Services.AddRazorPages();
        if (defaultOptions.Api)
        {
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(
                    "v1",
                    new OpenApiInfo { Title = $"{defaultOptions.Name} Api", Version = "v1" }
                );
            });
        }
        if (!defaultOptions.Management && defaultOptions.Api)
            return builder;

        builder.Services.RegisterModules();

        return builder;
    }

    internal static WebApplication RegisterDefaults(this WebApplication app)
    {
        var logger = Initializer.GetLogger();
        var defaultOptions = new DefaultOptions();
        app.Configuration.GetSection(DefaultOptions.SectionKey).Bind(defaultOptions);
        var enabled = defaultOptions.Management;

        app.UseHttpsRedirection().UseAuthorization();

        if (defaultOptions.Api)
        {
            app.RegisterEndpoints();
        }

        if (!enabled)
        {
            logger.LogInformation("Management is disabled...");
            return app;
        }

        logger.LogInformation("Starting management UI...");
        app.MapRazorPages();
        app.UseStaticFiles().UseRouting();

        if (!app.Environment.IsDevelopment())
            return app;
        app.UseExceptionHandler("/Error");

        return app;
    }
}
