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
        [ResponseCache(Duration = 3000)]
        public IActionResult GetByName(string name)
        {
            var countries = repository.GetAll()
                .Where(c => c.Name.ToLower().Contains(name.ToLower()));

            return Json(countries);
        }
    }
}
