namespace Frequency.Resources.Queue;

public class QueueModule : IModule {
    public IServiceCollection RegisterApiModule(IServiceCollection services) => services;

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints) => endpoints;
}