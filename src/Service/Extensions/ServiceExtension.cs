using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Microsoft.OpenApi.Models;
using Frequency.Common.Config;
using Frequency.Resources;

namespace Frequency.Extensions;

internal static class ServiceExtension {
    internal static WebApplicationBuilder RegisterStandardServices(
        this WebApplicationBuilder builder
    ) {
        var serviceOptions = new SystemConfig();
        var serverOptions = new ServerConfig();

        builder.Configuration.GetSection(SystemConfig.Key).Bind(serviceOptions);
        builder.Configuration.GetSection(ServerConfig.Key).Bind(serverOptions);
        switch (serviceOptions.Web) {
            case false when serviceOptions.Api:
                builder.Services.AddAntiforgery();
                return builder;
            case true:
                builder.Services.AddRazorPages();
                builder.Services.AddControllersWithViews();
                // TODO: Test after dotnet 8.0 release
                // builder.Services.AddRazorComponents()
                //     .AddServerComponents()
                //     .AddWebAssemblyComponents();
                builder.Services.AddBlazorise(options => { options.Immediate = true; })
                    .AddBootstrap5Providers()
                    .AddFontAwesomeIcons();
                break;
        }

        if (!serviceOptions.Api) {
            return builder;
        }

        builder.Services.AddControllers();
        builder.Services.RegisterModules();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSwaggerGen(options => {
            options.SwaggerDoc(
                "v1",
                new OpenApiInfo { Title = $"{serverOptions.Name}", Version = "v1" }
            );
        });

        return builder;
    }

    internal static WebApplication RegisterAppServices(this WebApplication app) {
        var logger = Initializer.GetLogger<WebApplication>();
        var serviceOptions = new SystemConfig();
        app.Configuration.GetSection(SystemConfig.Key).Bind(serviceOptions);

        switch (serviceOptions.Web) {
            case false when !serviceOptions.Api:
                return app;
            case false:
                logger.LogInformation("Web management is disabled...");
                return app;
        }

        app.UseRouting();
        app.UseAntiforgery();
        app.UseHttpsRedirection();
        app.UseAuthorization();

        if (serviceOptions.Api) {
            app.RegisterEndpoints();
        }

        logger.LogInformation("Starting web management UI...");
        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();
        app.MapRazorPages();
        app.MapControllers();
        app.MapFallbackToPage("/_Host");
        // TODO: Test after dotnet 8.0 release
        // app.MapRazorComponents<Server>()
        //     .AddServerRenderMode()
        //     .AddWebAssemblyRenderMode();

        if (!app.Environment.IsDevelopment()) {
            return app;
        }

        app.UseWebAssemblyDebugging();
        app.UseExceptionHandler("/Error");
        // app.MapFallback( context => {
        //     context.Response.Redirect("/404");
        //     return Task.CompletedTask;
        // });

        return app;
    }
}