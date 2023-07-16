namespace Proton.Frequency.Resources.Events;

public class EventsModule : IModule {
    public IServiceCollection RegisterApiModule(IServiceCollection services) {
        return services;
    }

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints) {
        return endpoints;
    }
}
