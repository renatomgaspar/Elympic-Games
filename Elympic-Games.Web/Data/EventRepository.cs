using Elympic_Games.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Elympic_Games.Web.Data
{
    public class EventRepository : GenericRepository<Event>, IEventRepository
    {
        private readonly DataContext _context;

        public EventRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Event> GetEventAsync(int id)
        {
            return await _context.Events
                .Include(e => e.Arena)
                .ThenInclude(a => a.City)
                .ThenInclude(c => c.Country)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<int> IsEventAlreadyCreated(Event eventObj)
        {
            var existentEvent = _context.Events
                    .AsNoTracking()
                    .FirstOrDefault(e => e.Id != eventObj.Id && e.Name == eventObj.Name);

            if (existentEvent != null)
            {
                return 0;
            }

            existentEvent = _context.Events
                    .AsNoTracking()
                    .FirstOrDefault(e => e.Id != eventObj.Id && 
                    e.StartDate <= eventObj.EndDate && e.EndDate >= eventObj.StartDate);

            if (existentEvent != null)
            {
                return 2;
            }

            return 1;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboArenas()
        {
            var list = _context.Arena
                        .Select(a => new SelectListItem
                        {
                            Text = a.Name + " - " + a.City.Country.Name,
                            Value = a.Id.ToString()
                        })
                        .OrderBy(a => a.Text)
                        .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "Select an Arena...",
                Value = string.Empty
            });

            return list;
        }
    }
}
