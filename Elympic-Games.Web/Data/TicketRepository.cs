using Elympic_Games.Web.Data.Entities;
using Elympic_Games.Web.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Elympic_Games.Web.Data
{
    public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public TicketRepository(
            DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Ticket> GetTicketAsync(int id)
        {
            return await _context.Tickets
                .Include(t => t.Event)
                .ThenInclude(e => e.GameType)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> IsTicketAlreadyCreated(Ticket ticket)
        {
            var existentTicket = _context.Tickets
                    .AsNoTracking()
                    .FirstOrDefault(a => a.Id != ticket.Id && a.EventId == ticket.EventId && a.TicketType == ticket.TicketType);

            if (existentTicket == null)
            {
                return false;
            }

            return true;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboEvents()
        {
            var list = _context.Events
                        .Select(gt => new SelectListItem
                        {
                            Text = gt.Name + " - " + gt.GameType.Name,
                            Value = gt.Id.ToString()
                        })
                        .OrderBy(gt => gt.Text)
                        .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "Select a Event...",
                Value = string.Empty
            });

            return list;
        }
    }
}
