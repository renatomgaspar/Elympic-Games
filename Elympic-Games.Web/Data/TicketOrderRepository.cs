using Elympic_Games.Web.Data.Entities;
using Elympic_Games.Web.Helpers;
using Elympic_Games.Web.Models.TicketOrders;
using Microsoft.EntityFrameworkCore;

namespace Elympic_Games.Web.Data
{
    public class TicketOrderRepository : GenericRepository<TicketOrder>, ITicketOrderRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public TicketOrderRepository(
            DataContext context, 
            IUserHelper userHelper) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task<IQueryable<TicketOrder>> GetTicketOrderAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);

            if (user == null)
            {
                return null;
            }
            else
            {
                if (await _userHelper.IsUserInRoleAsync(user, "Admin"))
                {
                    return _context.TicketOrders
                        .Include(o => o.Items)
                        .ThenInclude(p => p.Ticket)
                        .ThenInclude(t => t.Event)
                        .ThenInclude(e => e.GameType)
                        .OrderByDescending(o => o.OrderDate);
                }


                return _context.TicketOrders
                        .Include(o => o.Items)
                        .ThenInclude(p => p.Ticket)
                        .ThenInclude(t => t.Event)
                        .ThenInclude(e => e.GameType)
                        .Where(o => o.User == user)
                        .OrderByDescending(o => o.OrderDate);
            }
        }

        public async Task AddItemToOrderAsync(AddItemViewModel model, string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);

            if (user == null)
            {
                return;
            }

            var ticket = await _context.Tickets.FindAsync(model.TicketId);

            if (ticket == null)
            {
                return;
            }

            var cart = await _context.Carts
                .Where(c => c.User == user && c.Ticket == ticket)
                .FirstOrDefaultAsync();

            if (cart == null)
            {
                cart = new Cart
                {
                    Price = ticket.Price,
                    Ticket = ticket,
                    Quantity = model.Quantity,
                    User = user
                };

                _context.Carts.Add(cart);
            }
            else
            {
                cart.Quantity += model.Quantity;

                _context.Carts.Update(cart);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IQueryable<Cart>> GetCartAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);

            if (user == null)
            {
                return null;
            }

            return _context.Carts
                        .Include(p => p.Ticket)
                        .ThenInclude(t => t.Event)
                        .ThenInclude(e => e.GameType)
                        .Where(o => o.User == user)
                        .OrderBy(o => o.Ticket.Event.Name)
                        .ThenBy(o => o.Ticket.Event.GameType.Name);
        }

        public async Task DeleteTicketFromCartAsync(int id)
        {
            var cart = await _context.Carts.FindAsync(id);

            if (cart == null)
            {
                return;
            }

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
        }
    }
}
