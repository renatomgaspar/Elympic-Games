using Elympic_Games.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Elympic_Games.Web.Data
{
    public interface IMatchRepository : IGenericRepository<Match>
    {
        Task<Match> GetMatchAsync(int id);

        Task<bool> IsMaxMatchsAsync(Match match);

        Task<bool> ExistMatchInDatesAsync(Match match);

        Task<int> ExistMatchWithSameTeamsAsync(Match match);

        Task<IEnumerable<SelectListItem>> GetComboTeams(int gametypeId);
    }
}
