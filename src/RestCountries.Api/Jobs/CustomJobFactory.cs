using System;
using System.Threading.Tasks;
using Quartz;
using Quartz.Spi;

namespace RestCountries.Api
{
    public class CustomJobFactory : IJobFactory
    {
        private readonly IServiceProvider container;

        public CustomJobFactory(IServiceProvider container)
        {
            this.container = container;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return container.GetService(bundle.JobDetail.JobType) as IJob;
        }

        public void ReturnJob(IJob job)
        {
        }
    }
}