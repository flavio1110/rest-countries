using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace RestCountries.Api
{
    public class CountryRepository : ICountryRepository
    {
        private const string cacheKey = "countries-cache-key";

        private IEnumerable<Country> countries 
        {
            get => cache.Get(cacheKey) as IEnumerable<Country>
                ?? Enumerable.Empty<Country>();
        }
        private IMemoryCache cache;

        public CountryRepository(IMemoryCache cache)
        {
            this.cache = cache;
        }

        public async Task HydrateCache()
        {
            var countries = await GetCountries();
            Console.WriteLine("Refreshing Cache");
            cache.Set(cacheKey, countries, TimeSpan.FromMinutes(1));
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
    }
}
