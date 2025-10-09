using Elympic_Games.Web.Data.Entities;
using Elympic_Games.Web.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Elympic_Games.Web.Data
{
    public class CityRepository : GenericRepository<City>, ICityRepository
    {
        private readonly DataContext _context;

        public CityRepository(DataContext context) : base(context)
        {
            _context = context;;
        }

        public async Task<City> GetCityAsync(int id)
        {
            return await _context.Cities
                .Include(c => c.Country)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> IsCityAlreadyCreated(City city)
        {
            var existentCity = _context.Cities
                    .AsNoTracking()
                    .FirstOrDefault(c => c.Id != city.Id && c.Name == city.Name && c.CountryId == city.CountryId);

            if (existentCity == null)
            {
                return false;
            }

            return true;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboCountries()
        {
            var list = _context.Countries
                        .Select(gt => new SelectListItem
                        {
                            Text = gt.Name,
                            Value = gt.Id.ToString()
                        })
                        .OrderBy(gt => gt.Text)
                        .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "Select a Country...",
                Value = string.Empty
            });

            return list;
        }
    }
}

