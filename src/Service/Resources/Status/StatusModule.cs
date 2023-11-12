namespace Frequency.Resources.Status;

public class StatusModule : IModule {
    public IServiceCollection RegisterApiModule(IServiceCollection services) => services;

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints) => endpoints;
}