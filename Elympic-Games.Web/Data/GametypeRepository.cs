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
    }
}
