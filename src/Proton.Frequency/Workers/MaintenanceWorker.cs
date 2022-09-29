using System.Diagnostics;

namespace Proton.Frequency.Workers;

internal class MaintenanceWorker : BackgroundService
{
    private readonly ILogger<MaintenanceWorker> _logger;
    private IServiceProvider _services { get; }
    private readonly IConfiguration _configuration;

    public MaintenanceWorker(
        IServiceProvider services,
        ILogger<MaintenanceWorker> logger,
        IConfiguration configuration
    )
    {
        _services = services;
        _logger = logger;
        _configuration = configuration;
    }

    private async Task DoWork(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting Service process...");

        /*
        using (var scope = _services.CreateScope())
         {
             var scopedProcessingService =
                 scope.ServiceProvider
                     .GetRequiredService<IScopedProcessingService>();

             await scopedProcessingService.DoWork(stoppingToken);
         }
         */
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }

    override protected async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Consume Scoped Service Hosted Service running.");
        await DoWork(stoppingToken);
    }

    public override Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Performing some important cleanup...");

        base.StopAsync(stoppingToken);
        return Task.CompletedTask;
    }
}
