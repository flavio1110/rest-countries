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

        public async Task Execute(IJobExecutionContext context)
        {
            await countryRepository.HydrateCache();
        }
    }
}
