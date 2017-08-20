using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace RestCountries.Api
{
    public class CountryRepository : ICountryRepository
    {
        private readonly IEnumerable<Country> countries;

        public CountryRepository()
        {
            var jsonData = File.ReadAllText("countries_data.json");

            countries = JsonConvert.DeserializeObject<List<Country>>(jsonData);
        }

        public IEnumerable<Country> GetAll()
        {
            return countries;
        }
    }
}
