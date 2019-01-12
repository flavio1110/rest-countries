using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using System.Linq;

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

            services.AddHealthChecks()
                .AddCheck<CacheHeatlhCheck>("CacheStore", tags: new[] { "ready" });

            services.TryAddSingleton<ICountryRepository, CountryRepository>();
            services.AddHostedService<CacheHydratorService>();
        }

        public void Configure(IApplicationBuilder app)
        {

            app.UseCors(builder =>
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
            );

            app.UseHealthChecks("/health/live", new HealthCheckOptions
            {
                Predicate = _ => false // don't validate anything just say I'm alive                
            });

            app.UseHealthChecks("/health/ready", new HealthCheckOptions
            {
                Predicate = item => item.Tags?.Contains("ready") == true,
                ResponseWriter = (context, report) =>
                {
                    var json = JsonConvert.SerializeObject(new
                    {
                        status = report.Status.ToString(),
                        durationMs = report.TotalDuration.Milliseconds,
                        entries = report.Entries.Select(e => new
                        {
                            item = e.Key,
                            descrtiption = e.Value.Description
                        }).ToArray()
                    }, Formatting.Indented);

                    return context.Response.WriteAsync(json);
                }
            });

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestCountry API");
            });
        }
    }
}
