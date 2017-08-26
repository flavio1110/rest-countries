using System;
using System.Threading.Tasks;
using Quartz;

namespace RestCountries.Api
{
    [DisallowConcurrentExecution]
    public class JobHydrator : IJob
    {
        private readonly ICountryRepository countryRepository;

        public JobHydrator(ICountryRepository countryRepository)
        {
            this.countryRepository = countryRepository;
        }

        public Task Execute(IJobExecutionContext context)
        {
            return countryRepository.HydrateCache();
        }
    }
}
