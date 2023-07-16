using Proton.Frequency.Common.Helpers;

namespace Proton.Frequency.Resources;

public interface IModule {
    IServiceCollection RegisterApiModule(IServiceCollection services);
    IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints);
}

public static class ModuleExtensions {
    private static readonly List<IModule> RegisteredModules = new();

    public static IServiceCollection RegisterModules(this IServiceCollection services) {
        var modules = DiscoverModules();
        foreach (var module in modules) {
            module.RegisterApiModule(services);
            RegisteredModules.Add(module);
        }

        return services;
    }

    public static WebApplication RegisterApiEndpoints(this WebApplication app) {
        foreach (var module in RegisteredModules)
            module.MapEndpoints(app);

        return app;
    }

    private static IEnumerable<IModule> DiscoverModules() {
        return FactoryLoader.LoadClassInstance<IModule>();
    }
}