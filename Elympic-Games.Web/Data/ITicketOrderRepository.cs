using Elympic_Games.Web.Data.Entities;
using Elympic_Games.Web.Models.TicketOrders;

namespace Elympic_Games.Web.Data
{
    public interface ITicketOrderRepository : IGenericRepository<TicketOrder>
    {
        Task<IQueryable<TicketOrder>> GetTicketOrderAsync(string userName);

        Task AddItemToOrderAsync(AddItemViewModel model, string userName);

        Task<IQueryable<Cart>> GetCartAsync(string userName);

        Task DeleteTicketFromCartAsync(int id);
    }
}
