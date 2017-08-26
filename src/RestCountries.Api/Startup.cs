using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using RestCountries.Api;
using Swashbuckle.AspNetCore.Swagger;

namespace RestCountries.Api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "RestCountries" });
            });

            services.TryAddSingleton<ICountryRepository, CountryRepository>();
            services.TryAddSingleton<JobHydrator>();
            services.TryAddSingleton<IJobFactory, CustomJobFactory>();
        }

        public void Configure(IApplicationBuilder app,
            IApplicationLifetime appLifetime,
            IJobFactory jobFactory,
            ICountryRepository repository)
        {
            repository.HydrateCache().Wait();

            JobsConfig.Start(appLifetime, jobFactory).Wait();

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestCountry API");
            });
        }
    }
}
