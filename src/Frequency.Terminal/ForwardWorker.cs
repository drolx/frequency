using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Proton.Frequency.Terminal.Handlers;
using Proton.Frequency.Terminal.Helpers;

namespace Proton.Frequency.Terminal
{
    public class ForwardWorker : BackgroundService
    {
        private readonly ConfigKey _config;
        private readonly ILogger<ForwardWorker> _logger;
        private readonly WebSync _webSync;

        public ForwardWorker(
            ILogger<ForwardWorker> logger,
            ConfigKey config,
            WebSync webSync
            )
        {
            _logger = logger;
            _config = config;
            _webSync = webSync;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_config.IOT_MODE_ENABLE && _config.IOT_REMOTE_HOST_ENABLE)
                {
                    _logger.LogInformation("Start cloud forwarding session...");
                    await _webSync.Sync();
                }
                await Task.Delay(_config.IOT_MIN_REMOTE_FREQ, stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Shutting down cloud forwarding session...");

            await base.StopAsync(stoppingToken);
        }
    }
}
