using System.Collections.Generic;

namespace RestCountries.Api
{
    public interface ICountryRepository
    {
        IEnumerable<Country> GetAll();
    }    
}
