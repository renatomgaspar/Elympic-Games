using Elympic_Games.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Elympic_Games.Web.Data
{
    public class MatchRepository : GenericRepository<Match>, IMatchRepository
    {
        private readonly DataContext _context;

        public MatchRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Match> GetMatchAsync(int id)
        {
            return await _context.Matches
                .Include(m => m.TeamOne)
                .ThenInclude(t => t.Country)
                .Include(m => m.TeamTwo)
                .ThenInclude(t => t.Country)
                .Include(m => m.Event)
                .ThenInclude(e => e.GameType)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<bool> IsMaxMatchsAsync(Match match)
        {
            int matchsCount = _context.Matches
                .Where(m => m.EventId == match.EventId && m.Id != match.Id)
                .Count();

            if (matchsCount == 4)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> ExistMatchInDatesAsync(Match match)
        {
            return await _context.Matches
                .AnyAsync(m =>
                    m.EventId == match.EventId &&
                    m.Id != match.Id &&
                    m.StartDate < match.EndDate &&
                    m.EndDate > match.StartDate);
        }

        public async Task<int> ExistMatchWithSameTeamsAsync(Match match)
        {
            if (await _context.Matches
                .AnyAsync(m =>
                    m.EventId == match.EventId &&
                    m.Id != match.Id &&
                    m.TeamOneId == match.TeamOneId || m.TeamTwoId == match.TeamOneId))
            {
                return 1;
            }

            if (await _context.Matches
                .AnyAsync(m =>
                    m.EventId == match.EventId &&
                    m.Id != match.Id &&
                    m.TeamTwoId == match.TeamTwoId || m.TeamOneId == match.TeamTwoId))
            {
                return 2;
            }

            return 0;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboTeams(int gametypeId)
        {
            var list = _context.Teams
                       .Where(t => t.GameType.Id == gametypeId)
                       .Select(c => new SelectListItem
                       {
                           Text = c.Name + " - " + c.Country.Name,
                           Value = c.Id.ToString()
                       })
                       .OrderBy(c => c.Text)
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
