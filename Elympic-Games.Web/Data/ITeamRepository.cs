using Elympic_Games.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Elympic_Games.Web.Data
{
    public interface ITeamRepository : IGenericRepository<Team>
    {
        Task<Team> GetTeamAsync(int id);

        Task<Team?> TeamExistsByCountryAndGame(Team team, bool isNew);

        Task<bool> TeamManagerExistsInDifferentCountry(Team team, bool isNew);

        Task<IEnumerable<SelectListItem>> GetComboItems(string table);

        Task<bool> HasDependenciesAsync(int id);
    }
}
