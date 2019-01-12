using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading;
using System.Threading.Tasks;

namespace RestCountries.Api
{
    public class CacheHeatlhCheck : IHealthCheck
    {
        private readonly ICountryRepository countryRepository;

        public CacheHeatlhCheck(ICountryRepository countryRepository)
        {
            this.countryRepository = countryRepository;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var countriesCached = countryRepository.Count() > 0;

            var result = countriesCached
                ? new HealthCheckResult(
                    status: HealthStatus.Healthy,
                    description: "It's ok")
                : new HealthCheckResult(
                    status: HealthStatus.Unhealthy,
                    description: "Countries not yet cached");

            return Task.FromResult(result);
        }
    }
}
