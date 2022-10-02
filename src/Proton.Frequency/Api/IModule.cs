namespace Proton.Frequency.Api;

public interface IModule
{
    IServiceCollection RegisterApiModule(IServiceCollection services);
    IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints);
}

public static class ModuleExtensions
{
    private readonly static List<IModule> RegisteredModules = new List<IModule>();

    public static IServiceCollection RegisterModules(this IServiceCollection services)
    {
        var modules = DiscoverModules();
        foreach (var module in modules)
        {
            module.RegisterApiModule(services);
            RegisteredModules.Add(module);
        }

        return services;
    }

    public static WebApplication RegisterApiEndpoints(this WebApplication app) {
        foreach (var module in RegisteredModules)
        {
            module.MapEndpoints(app);
        }
        
        return app;
    }

    private static IEnumerable<IModule> DiscoverModules()
    {
        return typeof(IModule).Assembly
            .GetTypes()
            .Where(p => p.IsClass && p.IsAssignableTo(typeof(IModule)))
            .Select(Activator.CreateInstance)
            .Cast<IModule>();
    }
}
