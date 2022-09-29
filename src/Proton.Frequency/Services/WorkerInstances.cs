using Proton.Frequency.Workers;

namespace Proton.Frequency.Services;

internal static class WorkerInstances
{
    internal static IServiceCollection RegisterWorkersServices(this IServiceCollection services)
    {
        services.AddHostedService<MaintenanceWorker>();

        return services;
    }
}
