using Elympic_Games.Web.Data.Entities;
using Elympic_Games.Web.Models.Countries;
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

        public async Task<IEnumerable<ShowCountryViewModel>> GetAllCountriesWithMatches()
        {
            return await _context.Countries
                .Where(c => _context.Matches.Any(m => m.TeamOne.CountryId == c.Id || m.TeamTwo.CountryId == c.Id))
                .Select(c => new ShowCountryViewModel
                {
                    CountryId = c.Id,
                    CountryName = c.Name,
                    CountryImage = c.ImageFullPath,
                    Games = _context.Teams
                        .Where(t => t.CountryId == c.Id &&
                                    _context.Matches.Any(m => m.TeamOneId == t.Id || m.TeamTwoId == t.Id))
                        .Select(t => t.GameType.Name)
                        .Distinct()
                        .ToList()
                })
                .OrderBy(c => c.CountryName)
                .ToListAsync();
        }

        public async Task<Country?> CountryExistsByCode(string code)
        {
            return _context.Countries.AsNoTracking().FirstOrDefault(c => c.Code == code);
        }

        public async Task<bool> HasDependenciesAsync(int id)
        {
            return await _context.Cities.AnyAsync(s => s.Country.Id == id)
                || await _context.Teams.AnyAsync(s => s.Country.Id == id);
        }
    }
}
