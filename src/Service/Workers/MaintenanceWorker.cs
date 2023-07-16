namespace Frequency.Workers;

internal class MaintenanceWorker : BackgroundService {
    private readonly IConfiguration _configuration;
    private readonly ILogger<MaintenanceWorker> _logger;

    public MaintenanceWorker(
        IServiceProvider services,
        ILogger<MaintenanceWorker> logger,
        IConfiguration configuration
    ) {
        _services = services;
        _logger = logger;
        _configuration = configuration;
    }

    private IServiceProvider _services { get; }

    private async Task DoWork(CancellationToken stoppingToken) {
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
        while (!stoppingToken.IsCancellationRequested) {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(TimeSpan.FromSeconds(35), stoppingToken);
        }
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken) {
        _logger.LogInformation("Consume Scoped Service Hosted Service running.");
        await DoWork(stoppingToken);
    }

    public override Task StopAsync(CancellationToken stoppingToken) {
        _logger.LogInformation("Performing some important cleanup...");

        base.StopAsync(stoppingToken);
        return Task.CompletedTask;
    }
}