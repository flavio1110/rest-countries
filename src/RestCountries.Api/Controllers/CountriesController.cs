using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

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
        [ResponseCache(Duration = 3000)]
        public IActionResult All()
        {
            return Json(repository.GetAll());
        }

        [HttpGet]
        [Route("/name/{name}")]
        [ProducesResponseType(typeof(IEnumerable<Country>), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ResponseCache(Duration = 3000)]
        public IActionResult GetByName(string name)
        {
            var countries = repository.GetAll()
                .Where(c => c.Name.ToLower().Contains(name.ToLower()));

            if(!countries.Any())
                return NotFound();

            return Json(countries);
        }

        [HttpGet]
        [Route("/name/{name}/full")]
        [ProducesResponseType(typeof(Country), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ResponseCache(Duration = 3000)]
        public IActionResult GetByFullName(string name)
        {
            var country = repository.GetAll()
                .FirstOrDefault(c => string.Equals(c.Name, name, StringComparison.InvariantCultureIgnoreCase));

            if(country == null)
                return NotFound();

            return Json(country);
        }

        [HttpGet]
        [Route("/alpha/{code}")]
        [ProducesResponseType(typeof(Country), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ResponseCache(Duration = 3000)]
        public IActionResult GetByCode(string code)
        {
            var country = repository.GetAll()
                .FirstOrDefault(c => string.Equals(c.Alpha2Code, code, StringComparison.InvariantCultureIgnoreCase)
                    || string.Equals(c.Alpha3Code, code, StringComparison.InvariantCultureIgnoreCase));

            if(country == null)
                return NotFound();

            return Json(country);
        }

        [HttpGet]
        [Route("/currency/{currency}")]
        [ProducesResponseType(typeof(IEnumerable<Country>), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ResponseCache(Duration = 3000)]
        public IActionResult GetByCurrency([FromRoute]string currency)
        {
            var countries = repository.GetAll()
                .Where(c => c.Currencies != null &&
                    c.Currencies.Any(cur => string.Equals(cur.Code, currency, StringComparison.InvariantCultureIgnoreCase)));

            if(!countries.Any())
                return NotFound();

            return Json(countries);
        }

        [HttpGet]
        [Route("/lang/{languageCode}")]
        [ProducesResponseType(typeof(IEnumerable<Country>), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ResponseCache(Duration = 3000)]
        public IActionResult GetByLanguage([FromRoute]string languageCode)
        {
            var countries = repository.GetAll()
                .Where(c => c.Languages != null &&
                    c.Languages.Any(lang => string.Equals(lang.Iso639_1, languageCode, StringComparison.InvariantCultureIgnoreCase)
                    || string.Equals(lang.Iso639_2, languageCode, StringComparison.InvariantCultureIgnoreCase)));

            if(!countries.Any())
                return NotFound();

            return Json(countries);
        }

        [HttpGet]
        [Route("/callingcode/{callingCode}")]
        [ProducesResponseType(typeof(IEnumerable<Country>), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ResponseCache(Duration = 3000)]
        public IActionResult GetByCallingCode([FromRoute]string callingCode)
        {
            var countries = repository.GetAll()
                .Where(c => c.CallingCodes != null &&
                    c.CallingCodes.Any(code => string.Equals(code, callingCode, StringComparison.InvariantCultureIgnoreCase)));

            if(!countries.Any())
                return NotFound();

            return Json(countries);
        }

        [HttpGet]
        [Route("/region/{region}")]
        [ProducesResponseType(typeof(IEnumerable<Country>), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ResponseCache(Duration = 3000)]
        public IActionResult GetByRegion([FromRoute]string region)
        {
            var countries = repository.GetAll()
                .Where(c => string.Equals(c.Region, region, StringComparison.InvariantCultureIgnoreCase));

            if(!countries.Any())
                return NotFound();

            return Json(countries);
        }

        [HttpGet]
        [Route("/regionalblock/{blocAcronym}")]
        [ProducesResponseType(typeof(IEnumerable<Country>), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ResponseCache(Duration = 3000)]
        public IActionResult GetByRegionalBlock(string blocAcronym)
        {
            var countries = repository.GetAll()
                .Where(c => c.RegionalBlocs != null &&
                    c.RegionalBlocs.Any(block => string.Equals(block.Acronym, blocAcronym, StringComparison.InvariantCultureIgnoreCase)));



            if(!countries.Any())
                return NotFound();

            return Json(countries);
        }
    }
}
