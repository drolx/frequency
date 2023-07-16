using Microsoft.AspNetCore.Mvc;
using Proton.Frequency.Resources.Action.Endpoints;

namespace Proton.Frequency.Resources.Action;

public class ActionModule : IModule {
    public IServiceCollection RegisterApiModule(IServiceCollection services) {
        services.AddScoped<ActionManagement>();

        return services;
    }

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints) {
        endpoints.MapGet("/api/v1/actions", (ActionManagement actions) => actions.Get());
        endpoints.MapGet(
            "/api/v1/actions/{id:int}",
            (ActionManagement actions, [FromRoute] int id) => actions.GetById(id)
        );

        return endpoints;
    }
}