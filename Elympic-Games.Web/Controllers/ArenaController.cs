using Elympic_Games.Web.Data;
using Elympic_Games.Web.Helpers;
using Elympic_Games.Web.Models.Arenas;
using Elympic_Games.Web.Models.Cities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Elympic_Games.Web.Controllers
{
    public class ArenaController : Controller
    {
        private readonly IArenaRepository _arenaRepository;
        private readonly IConverterHelper _converterHelper;

        public ArenaController(
            IArenaRepository arenaRepository,
            IConverterHelper converterHelper)
        {
            _arenaRepository = arenaRepository;
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
            return View(_arenaRepository
                .GetAll()
                .Include(a => a.City)
                .ThenInclude(c => c.Country)
                .OrderBy(a => a.City.Name)
                .ThenBy(a => a.Name));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var arena = await _arenaRepository.GetArenaAsync(id.Value);
            if (arena == null)
            {
                return NotFound();
            }

            return View(arena);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var model = new ArenaViewModel
            {
                Cities = await _arenaRepository.GetComboCities()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ArenaViewModel model)
        {
            model.Cities = await _arenaRepository.GetComboCities();

            if (ModelState.IsValid)
            {
                var arena = _converterHelper.ToArena(model, true);

                if (model.TotalCapacity < model.AccessibleSeating)
                {
                    ModelState.AddModelError(string.Empty, "Total capacity must be higher than the number of accessible seating");
                    return View(model);
                }

                if (await _arenaRepository.IsArenaAlreadyCreated(arena))
                {
                    ModelState.AddModelError(string.Empty, "The Arena already exits in that City");
                    return View(model);
                }

                await _arenaRepository.CreateAsync(arena);
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

            var arena = await _arenaRepository.GetArenaAsync(id.Value);
            if (arena == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToArenaViewModel(arena);
            model.Cities = await _arenaRepository.GetComboCities();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ArenaViewModel model)
        {
            model.Cities = await _arenaRepository.GetComboCities();

            if (ModelState.IsValid)
            {
                try
                {
                    var arena = _converterHelper.ToArena(model, false);

                    if (model.TotalCapacity < model.AccessibleSeating)
                    {
                        ModelState.AddModelError(string.Empty, "Total capacity must be higher than the number of accessible seating");
                        return View(model);
                    }

                    if (await _arenaRepository.IsArenaAlreadyCreated(arena))
                    {
                        ModelState.AddModelError(string.Empty, "The Arena already exits in that City");
                        return View(model);
                    }

                    await _arenaRepository.UpdateAsync(arena);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _arenaRepository.ExistAsync(model.Id))
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

            var arena = await _arenaRepository.GetArenaAsync(id.Value);
            if (arena == null)
            {
                return NotFound();
            }

            return View(arena);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var team = await _arenaRepository.GetByIdAsync(id);

            await _arenaRepository.DeleteAsync(team);
            return RedirectToAction(nameof(Manage));
        }
    }
}
