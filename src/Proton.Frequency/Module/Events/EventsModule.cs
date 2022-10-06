namespace Proton.Frequency.Module.Events;

public class EventsModule : IModule
{
    public IServiceCollection RegisterApiModule(IServiceCollection services)
    {
        return services;
    }

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        return endpoints;
    }
}
