using Elympic_Games.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Elympic_Games.Web.Data
{
    public interface ICityRepository : IGenericRepository<City>
    {
        Task<City> GetCityAsync(int id);

        Task<bool> IsCityAlreadyCreated(City cityd);

        Task<IEnumerable<SelectListItem>> GetComboCountries();

        Task<bool> HasDependenciesAsync(int id);
    }
}
