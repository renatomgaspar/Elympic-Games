using Elympic_Games.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Elympic_Games.Web.Data
{
    public interface ITicketRepository : IGenericRepository<Ticket>
    {
        Task<Ticket> GetTicketAsync(int id);

        Task<bool> IsTicketAlreadyCreated(Ticket ticket);

        Task<IEnumerable<SelectListItem>> GetComboEvents();

        IEnumerable<SelectListItem> GetTicketsInEvent();

        Task<bool> HasDependenciesAsync(int id);
    }
}
