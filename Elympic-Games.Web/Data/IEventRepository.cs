using Elympic_Games.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Elympic_Games.Web.Data
{
    public interface IEventRepository : IGenericRepository<Event>
    {
        Task<Event> GetEventAsync(int id);

        Task<int> IsEventAlreadyCreated(Event eventObj);

        Task<IEnumerable<SelectListItem>> GetComboArenas();
    }
}
