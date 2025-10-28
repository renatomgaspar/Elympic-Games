using Elympic_Games.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Elympic_Games.Web.Data
{
    public class GametypeRepository : GenericRepository<GameType>, IGametypeRepository
    {
        private readonly DataContext _context;

        public GametypeRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<GameType?> GametypeExistsByName(string name)
        {
            return _context.GameTypes.AsNoTracking().FirstOrDefault(c => c.Name == name);
        }

        public async Task<bool> HasDependenciesAsync(int id)
        {
            return await _context.Teams.AnyAsync(s => s.GameType.Id == id)
                || await _context.Events.AnyAsync(s => s.GameType.Id == id);
        }
    }
}
