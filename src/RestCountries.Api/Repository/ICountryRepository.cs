using System;
using System.Collections.Generic;

namespace RestCountries.Api
{
    public interface ICountryRepository
    {
        IEnumerable<Country> GetAll();
        IEnumerable<Country> GetByPredicate(Func<Country, bool> predicate);
    }
}
