using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HealthCheckWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly HealthCheckSettings _settings;
        private HttpClient _client;

        public Worker(ILogger<Worker> logger, IOptions<HealthCheckSettings> options)
        {
            _logger = logger;
            _settings = options.Value;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _client = new HttpClient();
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = await _client.GetAsync(_settings.Url);

                if (result.IsSuccessStatusCode) _logger.LogInformation($"The web service is up. HTTP {result.StatusCode}");

                await Task.Delay(_settings.IntervalMs, stoppingToken);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _client.Dispose();
            return base.StopAsync(cancellationToken);
        }
    }
}
