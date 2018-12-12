using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Quartz.Spi;
using Swashbuckle.AspNetCore.Swagger;

namespace RestCountries.Api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMemoryCache();
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

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
            IJobFactory jobFactory)
        {
            JobsConfig.Start(appLifetime, jobFactory).Wait();

            app.UseCors(builder => 
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
            );
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestCountry API");
            });
        }
    }
}
