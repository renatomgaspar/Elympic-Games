using Elympic_Games.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Elympic_Games.Web.Data
{
    public interface IPlayerRepository : IGenericRepository<Player>
    {
        Task<Player> GetPlayerAsync(int id);

        Task<bool> IsTeamFull(Player player);

        Task<bool> IsActivePlayersFull(Player player);

        Task<IEnumerable<SelectListItem>> GetComboTeams(string teamManager);
    }
}
