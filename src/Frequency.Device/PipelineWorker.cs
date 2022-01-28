using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Proton.Frequency.Device.Handlers;
using Proton.Frequency.Device.Helpers;

namespace Proton.Frequency.Device
{
    public class PipelineWorker : BackgroundService
    {
        private readonly ConfigKey _config;
        private readonly ILogger<PipelineWorker> _logger;
        private readonly ReaderProcess _readerProcess;

        public PipelineWorker(
            ConfigKey config,
            ILogger<PipelineWorker> logger,
            ReaderProcess readerProcess
            )
        {
            _config = config;
            _logger = logger;
            _readerProcess = readerProcess;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Pipeline Serial conection initialization...");

                try
                {
                    _readerProcess.Initialize(stoppingToken).Wait(stoppingToken);
                    Task.Delay(TimeSpan.FromSeconds(10)).Wait(stoppingToken);
                }
                catch (Exception e)
                {
                    var error = $"{e.Message}";
                    _logger.LogInformation($"Focefully exiting current process...");
                }
            }

            return Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Shutting down Serial conection pipeline...");

            await base.StopAsync(stoppingToken);
        }
    }
}
