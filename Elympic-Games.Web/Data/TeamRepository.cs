using Elympic_Games.Web.Data.Entities;
using Elympic_Games.Web.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Elympic_Games.Web.Data
{
    public class TeamRepository : GenericRepository<Team>, ITeamRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public TeamRepository(
            DataContext context,
            IUserHelper userHelper) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task<Team> GetTeamAsync(int id)
        {
            return await _context.Teams
                .Include(t => t.GameType)
                .Include(t => t.Country)
                .Include(t => t.TeamManager)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Team?> TeamExistsByCountryAndGame(Team team, bool isNew)
        {
            if (isNew)
            {
                return _context.Teams
                    .AsNoTracking()
                    .FirstOrDefault(t => t.GameTypeId == team.GameTypeId && t.CountryId == team.CountryId);
            }

            return _context.Teams
                .AsNoTracking()
                .FirstOrDefault(t => t.GameTypeId == team.GameTypeId && t.CountryId == team.CountryId && t.Id != team.Id);
        }

        public async Task<bool> TeamManagerExistsInDifferentCountry(Team team, bool isNew)
        {
            Team teamManaging;

            if (isNew)
            {
                teamManaging = _context.Teams
                    .AsNoTracking()
                    .FirstOrDefault(t => t.TeamManagerId == team.TeamManagerId && t.CountryId != team.CountryId);
            }
            else
            {
                teamManaging = _context.Teams
                    .AsNoTracking()
                    .FirstOrDefault(t => t.TeamManagerId == team.TeamManagerId && t.CountryId != team.CountryId && t.Id != team.Id);
            }

            if (teamManaging == null)
            {
                return false;
            }

            return true;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboItems(string table)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            switch (table.ToLower())
            {
                case "gametypes":
                    list = _context.GameTypes
                        .Select(gt => new SelectListItem
                        {
                            Text = gt.Name,
                            Value = gt.Id.ToString()
                        })
                        .OrderBy(gt => gt.Text)
                        .ToList();
                    break;

                case "countries":
                    list = _context.Countries
                        .Select(c => new SelectListItem
                        {
                            Text = c.Name,
                            Value = c.Id.ToString()
                        })
                        .OrderBy(c => c.Text)
                        .ToList();
                    break;

                case "users":
                    list = (await _userHelper.GetUsersInRoleAsync("Team Manager")).ToList();
                    break;

                default:
                    break;
            }

            list.Insert(0, new SelectListItem
            {
                Text = "Select an Item...",
                Value = string.Empty
            });

            return list;
        }

        public async Task<bool> HasDependenciesAsync(int id)
        {
            return await _context.Matches.AnyAsync(s => s.TeamOne.Id == id || s.TeamTwo.Id == id)
                || await _context.Classifications.AnyAsync(s => s.Team.Id == id);
        }
    }
}
