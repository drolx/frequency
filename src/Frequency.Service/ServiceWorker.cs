namespace Proton.Frequency.Service;

public class ServiceWorker : BackgroundService
{
    private readonly ILogger<ServiceWorker> _logger;

    public ServiceWorker(ILogger<ServiceWorker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(5000, stoppingToken);
        }
    }
}

