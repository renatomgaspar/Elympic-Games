using Elympic_Games.Web.Data.Entities;
using Elympic_Games.Web.Models.Countries;

namespace Elympic_Games.Web.Data
{
    public interface ICountryRepository : IGenericRepository<Country>
    {
        Task<IEnumerable<ShowCountryViewModel>> GetAllCountriesWithMatches();

        Task<Country?> CountryExistsByCode(string code);
    }
}
