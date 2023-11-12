namespace Frequency.Resources.Events;

public class EventsModule : IModule {
    public IServiceCollection RegisterApiModule(IServiceCollection services) => services;

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints) => endpoints;
}