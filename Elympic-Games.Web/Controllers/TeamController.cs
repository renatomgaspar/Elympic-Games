using Elympic_Games.Web.Data;
using Elympic_Games.Web.Helpers;
using Elympic_Games.Web.Models.Teams;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Elympic_Games.Web.Controllers
{
    public class TeamController : Controller
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IConverterHelper _converterHelper;

        public TeamController(
            ITeamRepository teamRepository,
            IConverterHelper converterHelper)
        {
            _teamRepository = teamRepository;
            _converterHelper = converterHelper;
        }

        [Authorize]
        [Authorize(Roles = "Admin")]
        public IActionResult Manage()
        {
            return View(_teamRepository
                .GetAll()
                .Include(t => t.GameType)
                .Include(t => t.Country)
                .Include(t => t.TeamManager)
                .OrderBy(t => t.Country.Name)
                .ThenBy(t => t.GameType.Name));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _teamRepository.GetTeamAsync(id.Value);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var model = new TeamViewModel
            {
                GameTypes = await _teamRepository.GetComboItems("GameTypes"),
                Countries = await _teamRepository.GetComboItems("Countries"),
                Users = await _teamRepository.GetComboItems("Users")
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeamViewModel model)
        {
            model.GameTypes = await _teamRepository.GetComboItems("GameTypes");
            model.Countries = await _teamRepository.GetComboItems("Countries");
            model.Users = await _teamRepository.GetComboItems("Users");

            if (ModelState.IsValid)
            {
                var team = _converterHelper.ToTeam(model, true);

                if (await _teamRepository.TeamManagerExistsInDifferentCountry(team, true) == true)
                {
                    ModelState.AddModelError(string.Empty, "The User already have a country to manage");
                    return View(model);
                }

                if (await _teamRepository.TeamExistsByCountryAndGame(team, true) != null)
                {
                    ModelState.AddModelError(string.Empty, "This country already have a team with that game");
                    return View(model);
                }

                await _teamRepository.CreateAsync(team);
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

            var team = await _teamRepository.GetTeamAsync(id.Value);
            if (team == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToTeamViewModel(team);
            model.GameTypes = await _teamRepository.GetComboItems("GameTypes");
            model.Countries = await _teamRepository.GetComboItems("Countries");
            model.Users = await _teamRepository.GetComboItems("Users");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TeamViewModel model)
        {
            model.GameTypes = await _teamRepository.GetComboItems("GameTypes");
            model.Countries = await _teamRepository.GetComboItems("Countries");
            model.Users = await _teamRepository.GetComboItems("Users");

            if (ModelState.IsValid)
            {
                try
                {
                    var team = _converterHelper.ToTeam(model, false);

                    if (await _teamRepository.TeamManagerExistsInDifferentCountry(team, false) == true)
                    {
                        ModelState.AddModelError(string.Empty, "The User already have a country to manage");
                        return View(model);
                    }

                    if (await _teamRepository.TeamExistsByCountryAndGame(team, false) != null)
                    {
                        ModelState.AddModelError(string.Empty, "This country already have a team with that game");
                        return View(model);
                    }

                    await _teamRepository.UpdateAsync(team);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _teamRepository.ExistAsync(model.Id))
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

            var team = await _teamRepository.GetTeamAsync(id.Value);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var team = await _teamRepository.GetByIdAsync(id);

            await _teamRepository.DeleteAsync(team);
            return RedirectToAction(nameof(Manage));
        }
    }
}
