using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestCountries.Api
{
    public interface ICountryRepository
    {
        IEnumerable<Country> GetAll();
        IEnumerable<Country> GetByPredicate(Func<Country, bool> predicate);
        Task HydrateCache();
    }
}
