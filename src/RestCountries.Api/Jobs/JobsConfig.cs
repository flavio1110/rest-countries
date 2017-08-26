using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace RestCountries.Api
{
    public static class JobsConfig
    {
        public static async Task Start(IApplicationLifetime appLifetime, IJobFactory jobFactory)
        {
            var factory = new StdSchedulerFactory();
            var scheduler = await factory.GetScheduler();
            scheduler.JobFactory = jobFactory;

            await scheduler.Start();

            appLifetime.ApplicationStopped.Register(() => scheduler.Shutdown().Wait());

            IJobDetail job = JobBuilder.Create<JobHydrator>()
                .WithIdentity("jobHydrator", "defaultGroup")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("every30seconds", "defaultGroup")
                .StartAt(DateTimeOffset.FromUnixTimeSeconds(30))
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(30)
                    .RepeatForever())
                .Build();

            await scheduler.ScheduleJob(job, trigger);
        }
    }
}