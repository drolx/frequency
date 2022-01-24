namespace Proton.Frequency.Service.Process;

public class StateWorker : BackgroundService
{
    private readonly ILogger<StateWorker> _logger;
    public IServiceProvider Services { get; }

    public StateWorker(IServiceProvider services, ILogger<StateWorker> logger)
    {
        Services = services;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
           "Consume Scoped Service Hosted Service running.");

        DoWork(stoppingToken);
        return Task.CompletedTask;
    }

    private Task DoWork(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Starting Servicex process...");

        /*
        using (var scope = Services.CreateScope())
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
            Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }

        return Task.CompletedTask;
    }

    public override Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Performing some important cleanup...");

        base.StopAsync(stoppingToken);
        return Task.CompletedTask;
    }

}

