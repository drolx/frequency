namespace Proton.Frequency.Resources.Objects;

public class ObjectsModule : IModule {
    public IServiceCollection RegisterApiModule(IServiceCollection services) {
        return services;
    }

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints) {
        return endpoints;
    }
}