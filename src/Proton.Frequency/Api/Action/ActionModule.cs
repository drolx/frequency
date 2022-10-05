using Microsoft.AspNetCore.Mvc;
using Proton.Frequency.Api.Action.Endpoints;

namespace Proton.Frequency.Api.Action;

public class ActionModule : IModule
{
    public IServiceCollection RegisterApiModule(IServiceCollection services)
    {
        services.AddScoped<ActionManagement>();

        return services;
    }

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/v1/actions", handler: (ActionManagement actions) => actions.Get());
        endpoints.MapGet(
            "/api/v1/actions/{id:int}",
            handler: (ActionManagement actions, [FromRoute] int id) => actions.GetById(id)
        );

        return endpoints;
    }
}
