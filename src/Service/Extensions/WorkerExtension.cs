using Frequency.Workers;

namespace Frequency.Extensions;

internal static class WorkerExtension {
    internal static IServiceCollection RegisterWorkersServices(this IServiceCollection services) {
        services.AddHostedService<MaintenanceWorker>();

        return services;
    }
}