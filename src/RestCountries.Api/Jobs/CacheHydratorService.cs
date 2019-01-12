using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RestCountries.Api
{
    public class CacheHydratorService : IHostedService
    {
        private const int interval_in_seconds = 5;

        private readonly ILogger<CacheHydratorService> logger;
        private readonly ICountryRepository countryRepository;
        private readonly CancellationTokenSource cts = new CancellationTokenSource();
        private Task executingTask;

        public CacheHydratorService(ICountryRepository countryRepository, ILogger<CacheHydratorService> logger)
        {
            this.countryRepository = countryRepository;
            this.logger = logger;
        }

        private async Task Execute(CancellationToken cancellationToken)
        {
            do
            {
                var startTime = DateTime.UtcNow;
                logger.LogInformation($"Hydration started at {startTime}");

                await countryRepository.HydrateCache(cancellationToken);

                logger.LogInformation($"Hydration took: {(DateTime.UtcNow - startTime).Seconds} seconds");

                await Task.Delay(interval_in_seconds * 1000, cancellationToken);
            } while (!cts.IsCancellationRequested);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            executingTask = Execute(cts.Token);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            cts.Cancel();

            return Task.CompletedTask;
        }
    }
}
