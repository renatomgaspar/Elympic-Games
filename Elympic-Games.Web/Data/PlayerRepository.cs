using Elympic_Games.Web.Data.Entities;
using Elympic_Games.Web.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Elympic_Games.Web.Data
{
    public class PlayerRepository : GenericRepository<Player>, IPlayerRepository
    {
        private readonly DataContext _context;

        public PlayerRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Player> GetPlayerAsync(int id)
        {
            return await _context.Players
                .Include(p => p.Team)
                    .ThenInclude(t => t.Country)
                .Include(p => p.Team)
                    .ThenInclude(t => t.GameType)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> IsTeamFull(Player player)
        {
            var countTeamSizeAtm = await _context.Players
                .CountAsync(p => p.TeamId == player.TeamId && p.Id != player.Id);

            var team = await _context.Teams
                .Include(t => t.GameType)
                .FirstOrDefaultAsync(t => t.Id == player.TeamId);

            if ((countTeamSizeAtm + 1) > team.GameType.TeamSize)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> IsActivePlayersFull(Player player)
        {
            var countActivePlayerNoAtm = await _context.Players
                .CountAsync(p => p.TeamId == player.TeamId && p.IsPlaying);

            var team = await _context.Teams
                .Include(t => t.GameType)
                .FirstOrDefaultAsync(t => t.Id == player.TeamId);

            if (player.IsPlaying)
            {
                if (countActivePlayerNoAtm + 1 > team.GameType.ActivePlayerNo)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboTeams(string teamManager)
        {
            var list = _context.Teams
                .Where(t => t.TeamManager.UserName == teamManager)
                .Select(t => new SelectListItem
                {
                    Text = t.Name + " - " + t.Country.Name + " - " + t.GameType.Name,
                    Value = t.Id.ToString()
                })
                .OrderBy(t => t.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "Select a Team...",
                Value = string.Empty
            });

            return list;
        }
    }
}
