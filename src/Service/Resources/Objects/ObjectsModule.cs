namespace Frequency.Resources.Objects;

public class ObjectsModule : IModule {
    public IServiceCollection RegisterApiModule(IServiceCollection services) => services;

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints) => endpoints;
}