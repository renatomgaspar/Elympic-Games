using Elympic_Games.Web.Data;
using Elympic_Games.Web.Data.Entities;
using Elympic_Games.Web.Helpers;
using Elympic_Games.Web.Models.Cities;
using Elympic_Games.Web.Models.Players;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Elympic_Games.Web.Controllers
{
    public class CityController : Controller
    {
        private readonly ICityRepository _cityRepository;
        private readonly IConverterHelper _converterHelper;

        public CityController(
            ICityRepository cityRepository,
            IConverterHelper converterHelper)
        {
            _cityRepository = cityRepository;
            _converterHelper = converterHelper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [Authorize(Roles = "Admin")]
        public IActionResult Manage()
        {
            return View(_cityRepository
                .GetAll()
                .Include(c => c.Country)
                .OrderBy(c => c.Country.Name)
                .ThenBy(c => c.Name));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _cityRepository.GetCityAsync(id.Value);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var model = new CityViewModel
            {
                Countries = await _cityRepository.GetComboCountries()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CityViewModel model)
        {
            model.Countries = await _cityRepository.GetComboCountries();

            if (ModelState.IsValid)
            {
                var city = _converterHelper.ToCity(model, true);

                if (await _cityRepository.IsCityAlreadyCreated(city))
                {
                    ModelState.AddModelError(string.Empty, "There is already a city created with the same name in that Country");
                    return View(model);
                }

                await _cityRepository.CreateAsync(city);
                return RedirectToAction(nameof(Manage));
            }

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _cityRepository.GetCityAsync(id.Value);
            if (city == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToCityViewModel(city);
            model.Countries = await _cityRepository.GetComboCountries();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CityViewModel model)
        {
            model.Countries = await _cityRepository.GetComboCountries();

            if (ModelState.IsValid)
            {
                try
                {
                    var city = _converterHelper.ToCity(model, false);

                    if (await _cityRepository.IsCityAlreadyCreated(city))
                    {
                        ModelState.AddModelError(string.Empty, "There is already a city created with the same name in that Country");
                        return View(model);
                    }

                    await _cityRepository.UpdateAsync(city);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _cityRepository.ExistAsync(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Manage));
            }

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _cityRepository.GetCityAsync(id.Value);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var city = await _cityRepository.GetByIdAsync(id);

            if (city == null)
            {
                return NotFound();
            }

            if (await _cityRepository.HasDependenciesAsync(id))
            {
                ViewBag.ErrorTitle = $"{city.Name} can not be deleted!";
                ViewBag.ErrorMessage = $"The City already has Arenas!";
                return View("Error");
            }

            await _cityRepository.DeleteAsync(city);
            return RedirectToAction(nameof(Manage));
        }
    }
}
