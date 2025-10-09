using Elympic_Games.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Elympic_Games.Web.Data
{
    public interface IArenaRepository : IGenericRepository<Arena>
    {
        Task<Arena> GetArenaAsync(int id);

        Task<bool> IsArenaAlreadyCreated(Arena arena);

        Task<IEnumerable<SelectListItem>> GetComboCities();
    }
}
