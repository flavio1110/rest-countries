using System;
using Xunit;
using RestCountries.Api.Controllers;
using RestCountries.Api;
using Newtonsoft.Json;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using System.Linq;
using Shouldly;
using System.Collections.Generic;
using System.Net;

namespace RestCountries.Api.Tests
{
    public class CountriesControllerTests : IDisposable
    {
        private readonly TestServer server;
        private readonly HttpClient client;

        public CountriesControllerTests()
        {
            server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            client = server.CreateClient();
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllCountries()
        {
            var actual = await client.GetAsync("/All");
            
            var countries = await GetCountriesResponse(actual);

            countries.Count().ShouldBe(4);
        }

        [Fact]
        public async Task GetByName_WithName_ShouldReturnCountriesThatContainsThatName()
        {
            var actual = await client.GetAsync("/name/bra");
            
            var countries = await GetCountriesResponse(actual);

            countries.Count().ShouldBe(2);
            countries.Any(c => c.Name == "Brazil").ShouldBeTrue();
            countries.Any(c => c.Name == "Brazileia").ShouldBeTrue();
        }

        [Fact]
        public async Task GetByName_WithNonExistingName_ShouldReturnNotFound()
        {
            var actual = await client.GetAsync("/name/trump");
            
            ShouldBeNotFound(actual);
        }

        [Fact]
        public async Task GetByFullName_WithFullName_ShouldReturnCountriesThatHaveThatName()
        {
            var actual = await client.GetAsync("/name/brazil/full");
            
            var countries = await GetCountriesResponse(actual);

            countries.Count().ShouldBe(1);
            countries.Any(c => c.Name == "Brazil").ShouldBeTrue();
        }

        [Fact]
        public async Task GetByFullName_WithNonExistingName_ShouldReturnNotFound()
        {
            var actual = await client.GetAsync("/name/trumpland/full");
            
            ShouldBeNotFound(actual);
        }

        [Fact]
        public async Task GetByCode_WithApha2Code_ShouldReturnCountriesThatHaveThatCode()
        {
            var actual = await client.GetAsync("/alpha/nl");
            
            var countries = await GetCountriesResponse(actual);

            countries.Count().ShouldBe(1);
            countries.Any(c => c.Name == "Netherlands").ShouldBeTrue();
        }

        [Fact]
        public async Task GetByCode_WithApha3Code_ShouldReturnCountriesThatHaveThatCode()
        {
            var actual = await client.GetAsync("/alpha/nld");
            
            var countries = await GetCountriesResponse(actual);

            countries.Count().ShouldBe(1);
            countries.Any(c => c.Name == "Netherlands").ShouldBeTrue();
        }

        [Fact]
        public async Task GetByCode_WithNonExistingApha3Code_ShouldReturnCountriesThatHaveThatCode()
        {
            var actual = await client.GetAsync("/alpha/tmp");
            
            ShouldBeNotFound(actual);
        }

        [Fact]
        public async Task GetByCode_WithNonExistingApha2Code_ShouldReturnCountriesThatHaveThatCode()
        {
            var actual = await client.GetAsync("/alpha/tp");
            
            ShouldBeNotFound(actual);
        }

        [Fact]
        public async Task GetByCurrency_WithCurrencyCode_ShouldReturnCountriesThatHaveThatCode()
        {
            var actual = await client.GetAsync("/currency/usd");
            
            var countries = await GetCountriesResponse(actual);

            countries.Count().ShouldBe(1);
            countries.Any(c => c.Name == "United States of America").ShouldBeTrue();
        }

        [Fact]
        public async Task GetByCurrency_WithNonExistingCurrencyCode_ShouldReturnCountriesThatHaveThatCode()
        {
            var actual = await client.GetAsync("/currency/bitcoin");
            
            ShouldBeNotFound(actual);
        }

        [Fact]
        public async Task GetByLanguage_WithLanguageCode_ShouldReturnCountriesThatHaveThatCode()
        {
            var actual = await client.GetAsync("/lang/nl");
            
            var countries = await GetCountriesResponse(actual);

            countries.Count().ShouldBe(1);
            countries.Any(c => c.Name == "Netherlands").ShouldBeTrue();
        }

        [Fact]
        public async Task GetByLanguage_WithNonExistingLanguageCode_ShouldReturnCountriesThatHaveThatCode()
        {
            var actual = await client.GetAsync("/lang/jp");
            
            ShouldBeNotFound(actual);
        }

        [Fact]
        public async Task GetByCallingCode_WithCallingCode_ShouldReturnCountriesThatHaveThatCode()
        {
            var actual = await client.GetAsync("/callingcode/31");
            
            var countries = await GetCountriesResponse(actual);

            countries.Count().ShouldBe(1);
            countries.Any(c => c.Name == "Netherlands").ShouldBeTrue();
        }

        [Fact]
        public async Task GetByCallingCode_WithNonExistingCallingCode_ShouldReturnCountriesThatHaveThatCode()
        {
            var actual = await client.GetAsync("/callingcode/88");
            
            ShouldBeNotFound(actual);
        }

        [Fact]
        public async Task GetByRegion_WithRegionCode_ShouldReturnCountriesThatHaveThatCode()
        {
            var actual = await client.GetAsync("/region/europe");
            
            var countries = await GetCountriesResponse(actual);

            countries.Count().ShouldBe(1);
            countries.Any(c => c.Name == "Netherlands").ShouldBeTrue();
        }

        [Fact]
        public async Task GetByRegion_WithNonExistingRegionCode_ShouldReturnCountriesThatHaveThatCode()
        {
            var actual = await client.GetAsync("/region/africa");
            
            ShouldBeNotFound(actual);
        }

        [Fact]
        public async Task GetByRegionalBlock_WithRegionalBlockCode_ShouldReturnCountriesThatHaveThatCode()
        {
            var actual = await client.GetAsync("/regionalblock/eu");
            
            var countries = await GetCountriesResponse(actual);

            countries.Count().ShouldBe(1);
            countries.Any(c => c.Name == "Netherlands").ShouldBeTrue();
        }

        [Fact]
        public async Task GetByRegionalBlock_WithNonExistingRegionalBlockCode_ShouldReturnCountriesThatHaveThatCode()
        {
            var actual = await client.GetAsync("/regionalblock/wtf");
            
            ShouldBeNotFound(actual);
        }

        private void ShouldBeNotFound(HttpResponseMessage response)
        {
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        private async Task<IEnumerable<Country>> GetCountriesResponse(HttpResponseMessage response)
        {
            response.IsSuccessStatusCode.ShouldBeTrue();

            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IEnumerable<Country>>(responseContent);
        }

        public void Dispose()
        {
            client.Dispose();
            server.Dispose();
        }
    }
}
