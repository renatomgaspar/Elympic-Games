using Elympic_Games.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Elympic_Games.Web.Data
{
    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        private readonly DataContext _context;

        public CountryRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Country?> CountryExistsByCode(string code)
        {
            return _context.Countries.AsNoTracking().FirstOrDefault(c => c.Code == code);
        }
    }
}
