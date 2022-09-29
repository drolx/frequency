using System.Diagnostics;

namespace Proton.Frequency.Workers;

internal class MaintenanceWorker : BackgroundService
{
    private readonly ILogger<MaintenanceWorker> _logger;
    private readonly IConfiguration _configuration;

    public MaintenanceWorker(ILogger<MaintenanceWorker> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    override protected async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var test = _configuration.GetValue<bool>("Server");
            _logger.LogInformation(
                "Maintenance Worker running at: {time}  - {test}",
                DateTimeOffset.Now,
                test.ToString()
            );
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
}
