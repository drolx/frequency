using Proton.Frequency.Workers;

namespace Proton.Frequency.Extensions;

internal static class WorkerExtension {
    internal static IServiceCollection RegisterWorkersServices(this IServiceCollection services) {
        services.AddHostedService<MaintenanceWorker>();

        return services;
    }
}