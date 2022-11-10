namespace Proton.Frequency.Module.Queue;

public class QueueModule : IModule {
    public IServiceCollection RegisterApiModule(IServiceCollection services) {
        return services;
    }

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints) {
        return endpoints;
    }
}
