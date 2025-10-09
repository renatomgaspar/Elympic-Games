using Elympic_Games.Web.Data;
using Elympic_Games.Web.Helpers;
using Elympic_Games.Web.Models.Players;
using Elympic_Games.Web.Models.Teams;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Elympic_Games.Web.Controllers
{
    public class PlayerController : Controller
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IConverterHelper _converterHelper;

        public PlayerController(
            IPlayerRepository playerRepository,
            IConverterHelper converterHelper)
        {
            _playerRepository = playerRepository;
            _converterHelper = converterHelper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [Authorize(Roles = "Team Manager")]
        public IActionResult Manage()
        {
            return View(_playerRepository
                .GetAll()
                .Include(p => p.Team)
                    .ThenInclude(t => t.Country)
                .Include(p => p.Team)
                    .ThenInclude(t => t.GameType)
                .Where(p => p.Team.TeamManager.Email == this.User.Identity.Name)
                .OrderBy(p => p.Team.Country.Name)
                .ThenBy(p => p.Team.GameType.Name)
                .ThenBy(p => p.FirstName)
                .ThenBy(p => p.LastName));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _playerRepository.GetPlayerAsync(id.Value);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        [Authorize(Roles = "Team Manager")]
        public async Task<IActionResult> Create()
        {
            var model = new PlayerViewModel
            {
                Teams = await _playerRepository.GetComboTeams(this.User.Identity.Name)
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlayerViewModel model)
        {
            model.Teams = await _playerRepository.GetComboTeams(this.User.Identity.Name);

            if (ModelState.IsValid)
            {
                var player = _converterHelper.ToPlayer(model, true);

                if (await _playerRepository.IsTeamFull(player))
                {
                    ModelState.AddModelError(string.Empty, "The team is already full. You can not register more players");
                    return View(model);
                }

                if (player.IsPlaying == true && await _playerRepository.IsActivePlayersFull(player))
                {
                    ModelState.AddModelError(string.Empty, "The team can not accept more Active Players");
                    return View(model);
                }

                await _playerRepository.CreateAsync(player);
                return RedirectToAction(nameof(Manage));
            }

            return View(model);
        }

        [Authorize(Roles = "Team Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _playerRepository.GetPlayerAsync(id.Value);
            if (player == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToPlayerViewModel(player);
            model.Teams = await _playerRepository.GetComboTeams(this.User.Identity.Name);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PlayerViewModel model)
        {
            model.Teams = await _playerRepository.GetComboTeams(this.User.Identity.Name);

            if (ModelState.IsValid)
            {
                try
                {
                    var player = _converterHelper.ToPlayer(model, false);

                    if (await _playerRepository.IsTeamFull(player))
                    {
                        ModelState.AddModelError(string.Empty, "The team is already full. You can not register more players");
                        return View(model);
                    }

                    if (player.IsPlaying == true && await _playerRepository.IsActivePlayersFull(player))
                    {
                        ModelState.AddModelError(string.Empty, "The team can not accept more Active Players");
                        return View(model);
                    }

                    await _playerRepository.UpdateAsync(player);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _playerRepository.ExistAsync(model.Id))
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

        [Authorize(Roles = "Team Manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _playerRepository.GetPlayerAsync(id.Value);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var player = await _playerRepository.GetByIdAsync(id);

            await _playerRepository.DeleteAsync(player);
            return RedirectToAction(nameof(Manage));
        }
    }
}
