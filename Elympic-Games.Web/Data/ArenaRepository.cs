using Elympic_Games.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Elympic_Games.Web.Data
{
    public class ArenaRepository : GenericRepository<Arena>, IArenaRepository
    {
        private readonly DataContext _context;

        public ArenaRepository(DataContext context) : base(context)
        {
            _context = context; ;
        }

        public async Task<Arena> GetArenaAsync(int id)
        {
            return await _context.Arena
                .Include(a => a.City)
               .ThenInclude(c => c.Country)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> IsArenaAlreadyCreated(Arena arena)
        {
            var existentArena = _context.Arena
                    .AsNoTracking()
                    .FirstOrDefault(a => a.Id != arena.Id && a.Name == arena.Name && a.CityId == arena.CityId);

            if (existentArena == null)
            {
                return false;
            }

            return true;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboCities()
        {
            var list = _context.Cities
                        .Select(gt => new SelectListItem
                        {
                            Text = gt.Name,
                            Value = gt.Id.ToString()
                        })
                        .OrderBy(gt => gt.Text)
                        .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "Select a City...",
                Value = string.Empty
            });

            return list;
        }
    }
}
