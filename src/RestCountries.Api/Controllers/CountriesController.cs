using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestCountries.Api.Controllers
{
    [Route("")]
    public class CountriesController : Controller
    {
        private readonly ICountryRepository repository;

        public CountriesController(ICountryRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Get()
        {
            return Redirect("swagger");
        }

        [HttpGet]
        [Route("All")]
        [ProducesResponseType(typeof(IEnumerable<Country>), 200)]
        //[ResponseCache(Duration = 120)]
        public IActionResult All()
        {
            return ListResponse(repository.GetAll());
        }

        [HttpGet]
        [Route("/name/{name}")]
        [ProducesResponseType(typeof(IEnumerable<Country>), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ResponseCache(Duration = 120)]
        public IActionResult GetByName(string name)
        {
            var countries = repository.GetByPredicate(
                c => c.Name.IndexOf(name, StringComparison.InvariantCultureIgnoreCase) >= 0);

            return ListResponse(countries);
        }

        [HttpGet]
        [Route("/name/{name}/full")]
        [ProducesResponseType(typeof(IEnumerable<Country>), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ResponseCache(Duration = 120)]
        public IActionResult GetByFullName(string name)
        {
            var countries = repository.GetByPredicate(
                c => string.Equals(c.Name, name, StringComparison.InvariantCultureIgnoreCase));

            return ListResponse(countries);
        }

        [HttpGet]
        [Route("/alpha/{code}")]
        [ProducesResponseType(typeof(IEnumerable<Country>), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ResponseCache(Duration = 120)]
        public IActionResult GetByCode(string code)
        {
            var countries = repository.GetByPredicate(c => string.Equals(c.Alpha2Code, code, StringComparison.InvariantCultureIgnoreCase)
                    || string.Equals(c.Alpha3Code, code, StringComparison.InvariantCultureIgnoreCase));

            return ListResponse(countries);
        }

        [HttpGet]
        [Route("/currency/{currency}")]
        [ProducesResponseType(typeof(IEnumerable<Country>), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ResponseCache(Duration = 120)]
        public IActionResult GetByCurrency([FromRoute]string currency)
        {
            var countries = repository.GetByPredicate(
                c => c.Currencies?.Any(cur => string.Equals(cur.Code, currency, StringComparison.InvariantCultureIgnoreCase)) == true);

            return ListResponse(countries);
        }

        [HttpGet]
        [Route("/lang/{languageCode}")]
        [ProducesResponseType(typeof(IEnumerable<Country>), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ResponseCache(Duration = 120)]
        public IActionResult GetByLanguage([FromRoute]string languageCode)
        {
            var countries = repository.GetByPredicate(
                c => c.Languages?.Any(lang => string.Equals(lang.Iso639_1, languageCode, StringComparison.InvariantCultureIgnoreCase)
                    || string.Equals(lang.Iso639_2, languageCode, StringComparison.InvariantCultureIgnoreCase)) == true);

            return ListResponse(countries);
        }

        [HttpGet]
        [Route("/callingcode/{callingCode}")]
        [ProducesResponseType(typeof(IEnumerable<Country>), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ResponseCache(Duration = 120)]
        public IActionResult GetByCallingCode([FromRoute]string callingCode)
        {
            var countries = repository.GetByPredicate(
                c => c.CallingCodes != null &&
                    c.CallingCodes.Any(code => string.Equals(code, callingCode, StringComparison.InvariantCultureIgnoreCase)));

            return ListResponse(countries);
        }

        [HttpGet]
        [Route("/region/{region}")]
        [ProducesResponseType(typeof(IEnumerable<Country>), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ResponseCache(Duration = 120)]
        public IActionResult GetByRegion([FromRoute]string region)
        {
            var countries = repository.GetByPredicate(
                c => string.Equals(c.Region, region, StringComparison.InvariantCultureIgnoreCase));

            return ListResponse(countries);
        }

        [HttpGet]
        [Route("/regionalblock/{blocAcronym}")]
        [ProducesResponseType(typeof(IEnumerable<Country>), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ResponseCache(Duration = 120)]
        public IActionResult GetByRegionalBlock(string blocAcronym)
        {
            var countries = repository.GetByPredicate(
                c => c.RegionalBlocs?.Any(block => string.Equals(block.Acronym, blocAcronym, StringComparison.InvariantCultureIgnoreCase)) == true);

            return ListResponse(countries);
        }

        private IActionResult ListResponse(IEnumerable<Country> countries)
        {
            return countries != null && countries.Any()
                ? (IActionResult)Json(countries)
                : NotFound();
        }
    }
}
