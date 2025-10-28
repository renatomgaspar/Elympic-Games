using Elympic_Games.Web.Data.Entities;
using Elympic_Games.Web.Models.Events;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Elympic_Games.Web.Data
{
    public interface IEventRepository : IGenericRepository<Event>
    {
        Task<List<EventDto>> GetEvents();

        Task<Event> GetEventAsync(int id);

        Task<int> IsEventAlreadyCreated(Event eventObj);

        Task<IEnumerable<SelectListItem>> GetComboItems(string table);
    }
}
