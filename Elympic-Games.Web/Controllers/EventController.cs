using Elympic_Games.Web.Data;
using Elympic_Games.Web.Data.Entities;
using Elympic_Games.Web.Helpers;
using Elympic_Games.Web.Models.Arenas;
using Elympic_Games.Web.Models.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Elympic_Games.Web.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventRepository _eventRepository;
        private readonly IArenaRepository _arenaRepository;
        private readonly IConverterHelper _converterHelper;

        public EventController(
            IEventRepository eventRepository,
            IArenaRepository arenaRepository,
            IConverterHelper converterHelper)
        {
            _eventRepository = eventRepository;
            _arenaRepository = arenaRepository;
            _converterHelper = converterHelper;
        }

        public IActionResult Index()
        {
            return View(_eventRepository
                .GetAll()
                .Include(e => e.GameType)
                .Include(e => e.Arena)
                .ThenInclude(a => a.City)
                .ThenInclude(c => c.Country)
                .OrderBy(e => e.StartDate));
        }

        [Authorize]
        [Authorize(Roles = "Admin")]
        public IActionResult Manage()
        {
            return View(_eventRepository
                .GetAll()
                .Include(e => e.GameType)
                .Include(e => e.Arena)
                .ThenInclude(a => a.City)
                .ThenInclude(c => c.Country)
                .OrderBy(e => e.StartDate));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventObj = await _eventRepository.GetEventAsync(id.Value);
            if (eventObj == null)
            {
                return NotFound();
            }

            return View(eventObj);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var model = new EventViewModel
            {
                GameTypes = await _eventRepository.GetComboItems("gametypes"),
                Arenas = await _eventRepository.GetComboItems("arenas"),
                StartDate = DateTime.Now.Date,
                EndDate = DateTime.Now.Date,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventViewModel model)
        {
            model.GameTypes = await _eventRepository.GetComboItems("gametypes");
            model.Arenas = await _eventRepository.GetComboItems("arenas");

            if (ModelState.IsValid)
            {
                var eventObj = _converterHelper.ToEvent(model, true);

                var arena = await _arenaRepository.GetArenaAsync(eventObj.ArenaId);

                eventObj.AvailableSeats = arena.TotalCapacity - arena.AccessibleSeating;
                eventObj.AvailableAccessibleSeats = arena.AccessibleSeating;

                if (model.StartDate < DateTime.Now || model.EndDate < DateTime.Now)
                {
                    ModelState.AddModelError(string.Empty, "Dates must in the future");
                    return View(model);
                }

                if (model.StartDate > model.EndDate)
                {
                    ModelState.AddModelError(string.Empty, "The start date must be before the end date");
                    return View(model);
                }

                int result = await _eventRepository.IsEventAlreadyCreated(eventObj);

                if (result == 0)
                {
                    ModelState.AddModelError(string.Empty, "The Event Already exists");
                    return View(model);
                }
                else if (result == 2)
                {
                    ModelState.AddModelError(string.Empty, "There is already an Event created in the range of the inserted dates");
                    return View(model);
                }

                await _eventRepository.CreateAsync(eventObj);
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

            var eventObj = await _eventRepository.GetEventAsync(id.Value);
            if (eventObj == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToEventViewModel(eventObj);
            model.GameTypes = await _eventRepository.GetComboItems("gametypes");
            model.Arenas = await _eventRepository.GetComboItems("arenas");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EventViewModel model)
        {
            model.GameTypes = await _eventRepository.GetComboItems("gametypes");
            model.Arenas = await _eventRepository.GetComboItems("arenas");

            if (ModelState.IsValid)
            {
                try
                {
                    var eventObj = _converterHelper.ToEvent(model, false);

                    if (model.StartDate > model.EndDate)
                    {
                        ModelState.AddModelError(string.Empty, "The start date must be before the end date");
                        return View(model);
                    }

                    int result = await _eventRepository.IsEventAlreadyCreated(eventObj);

                    if (result == 0)
                    {
                        ModelState.AddModelError(string.Empty, "The Event Already exists");
                        return View(model);
                    }
                    else if (result == 2)
                    {
                        ModelState.AddModelError(string.Empty, "There is already an Event created in the range of the dates typed");
                        return View(model);
                    }

                    await _eventRepository.UpdateAsync(eventObj);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _eventRepository.ExistAsync(model.Id))
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

            var eventObj = await _eventRepository.GetEventAsync(id.Value);
            if (eventObj == null)
            {
                return NotFound();
            }

            return View(eventObj);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventObj = await _eventRepository.GetEventAsync(id);

            if (eventObj == null)
            {
                return NotFound();
            }

            if (await _eventRepository.HasDependenciesAsync(id))
            {
                ViewBag.ErrorTitle = $"{eventObj.Name} - {eventObj.GameType.Name} can not be deleted!";
                ViewBag.ErrorMessage = $"The Event has already Matches, Classifications or Tickets!";
                return View("Error");
            }

            await _eventRepository.DeleteAsync(eventObj);
            return RedirectToAction(nameof(Manage));
        }
    }
}
