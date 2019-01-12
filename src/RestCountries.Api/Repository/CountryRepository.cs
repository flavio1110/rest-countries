using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RestCountries.Api
{
    public class CountryRepository : ICountryRepository
    {
        private const string cacheKey = "countries-cache-key";
        private readonly IMemoryCache cache;

        private IEnumerable<Country> countries 
        {
            get => cache.Get(cacheKey) as IEnumerable<Country>
                ?? Enumerable.Empty<Country>();
        }

        public CountryRepository(IMemoryCache cache)
        {
            this.cache = cache;
        }

        public async Task HydrateCache(CancellationToken cancellationToken)
        {
            // very costly operation
            await Task.Delay(30 * 1000);

            var countries = await GetCountries();
            cache.Set(cacheKey, countries, TimeSpan.FromMinutes(1));

            await Task.CompletedTask;
        }

        private async Task<IEnumerable<Country>> GetCountries()
        {
            var jsonData = await File.ReadAllTextAsync("countries_data.json");
            return  JsonConvert.DeserializeObject<List<Country>>(jsonData);
        }

        public IEnumerable<Country> GetAll()
        {
            return countries;
        }

        public IEnumerable<Country> GetByPredicate(Func<Country, bool> predicate)
        {
            return countries.Where(predicate);
        }

        public long Count()
        {
            return countries.Count();
        }
    }
}
