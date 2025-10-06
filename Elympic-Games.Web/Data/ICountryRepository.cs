using Elympic_Games.Web.Data.Entities;

namespace Elympic_Games.Web.Data
{
    public interface ICountryRepository : IGenericRepository<Country>
    {
        Task<Country?> CountryExistsByCode(string code);
    }
}
