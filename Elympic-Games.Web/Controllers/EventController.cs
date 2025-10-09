using Elympic_Games.Web.Data;
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
        private readonly IConverterHelper _converterHelper;

        public EventController(
            IEventRepository eventRepository,
            IConverterHelper converterHelper)
        {
            _eventRepository = eventRepository;
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
            return View(_eventRepository
                .GetAll()
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
                Arenas = await _eventRepository.GetComboArenas()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventViewModel model)
        {
            model.Arenas = await _eventRepository.GetComboArenas();

            if (ModelState.IsValid)
            {
                var eventObj = _converterHelper.ToEvent(model, true);

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
            model.Arenas = await _eventRepository.GetComboArenas();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EventViewModel model)
        {
            model.Arenas = await _eventRepository.GetComboArenas();

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
            var eventObj = await _eventRepository.GetByIdAsync(id);

            await _eventRepository.DeleteAsync(eventObj);
            return RedirectToAction(nameof(Manage));
        }
    }
}
