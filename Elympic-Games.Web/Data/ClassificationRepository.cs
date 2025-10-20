using Elympic_Games.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Elympic_Games.Web.Data
{
    public class ClassificationRepository : GenericRepository<Classification>, IClassificationRepository
    {
        private readonly DataContext _context;

        public ClassificationRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Classification>> GetClassificationsForAllAsync(int eventId)
        {
            var eventObj = await _context.Events.FirstOrDefaultAsync(e => e.Id == eventId);

            if (eventObj == null)
            {
                return new List<Classification>();
            }
                
            var classifications = await _context.Classifications
                .Include(c => c.Event)
                .Include(c => c.Team)
                    .ThenInclude(t => t.Country)
                .Where(c => c.Event.Name == eventObj.Name)
                .ToListAsync();

            var grouped = classifications
                .GroupBy(c => c.Team.Country)
                .Select(g => new Classification
                {
                    Team = new Team
                    {
                        Country = g.Key,
                        CountryId = g.Key.Id,
                        Name = g.Key.Name
                    },
                    Points = g.Sum(x => x.Points),
                    Rank = 0,
                    Event = eventObj
                })
                .OrderByDescending(c => c.Points)
                .ToList();

            int rank = 1;
            foreach (var c in grouped)
            {
                c.Rank = rank++;
            }

            return grouped;
        }


        public async Task<List<Classification>> GetClassificationsByEventIdAsync(int eventId)
        {
            return await _context.Classifications
                .Where(c => c.EventId == eventId)
                .Include(c => c.Event)
                .Include(c => c.Team)
                .ThenInclude(t => t.Country)
                .ToListAsync();
        }

        public async Task CreateClassification(Match match)
        {
            Classification classification = null;

            if (match.TeamOneScore > match.TeamTwoScore)
            {
                int numberOfMatches = await _context.Matches.Where(m => m.EventId == match.EventId
                    && (m.TeamTwoId == match.TeamTwoId || m.TeamOneId == match.TeamTwoId)).CountAsync();

                switch (numberOfMatches)
                {
                    case 1:
                        classification = new Classification
                        {
                            Id = 0,
                            Rank = 8,
                            Points = 10,
                            Event = match.Event,
                            Team = match.TeamTwo,
                        };

                        await CreateAsync(classification);

                        break;
                    case 2:
                        classification = new Classification
                        {
                            Id = 0,
                            Rank = 4,
                            Points = 20,
                            Event = match.Event,
                            Team = match.TeamTwo,
                        };

                        await CreateAsync(classification);

                        break;
                    case 3:
                        classification = new Classification
                        {
                            Id = 0,
                            Rank = 2,
                            Points = 30,
                            Event = match.Event,
                            Team = match.TeamTwo,
                        };

                        await CreateAsync(classification);

                        classification = new Classification
                        {
                            Id = 0,
                            Rank = 1,
                            Points = 50,
                            Event = match.Event,
                            Team = match.TeamOne,
                        };

                        await CreateAsync(classification);

                        break;
                    default:
                        break;
                }
            }
            else
            {
                int numberOfMatches = await _context.Matches.Where(m => m.EventId == match.EventId
                    && (m.TeamTwoId == match.TeamOneId || m.TeamOneId == match.TeamOneId)).CountAsync();

                switch (numberOfMatches)
                {
                    case 1:
                        classification = new Classification
                        {
                            Id = 0,
                            Rank = 8,
                            Points = 10,
                            Event = match.Event,
                            Team = match.TeamOne,
                        };

                        await CreateAsync(classification);

                        break;
                    case 2:
                        classification = new Classification
                        {
                            Id = 0,
                            Rank = 4,
                            Points = 20,
                            Event = match.Event,
                            Team = match.TeamOne,
                        };

                        await CreateAsync(classification);

                        break;
                    case 3:
                        classification = new Classification
                        {
                            Id = 0,
                            Rank = 2,
                            Points = 30,
                            Event = match.Event,
                            Team = match.TeamOne,
                        };

                        await CreateAsync(classification);

                        classification = new Classification
                        {
                            Id = 0,
                            Rank = 1,
                            Points = 50,
                            Event = match.Event,
                            Team = match.TeamTwo,
                        };

                        await CreateAsync(classification);

                        break;
                    default:
                        break;
                }
            }
        }
    }
}
