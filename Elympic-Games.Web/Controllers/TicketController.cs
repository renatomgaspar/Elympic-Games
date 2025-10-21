using Elympic_Games.Web.Data;
using Elympic_Games.Web.Helpers;
using Elympic_Games.Web.Models.Tickets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Elympic_Games.Web.Controllers
{
    public class TicketController : Controller
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IConverterHelper _converterHelper;

        public TicketController(
            ITicketRepository ticketRepository,
            IConverterHelper converterHelper)
        {
            _ticketRepository = ticketRepository;
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
            return View(_ticketRepository
                .GetAll()
                .Include(t => t.Event)
                .ThenInclude(t => t.GameType)
                .OrderBy(a => a.Event.Name)
                .ThenBy(t => t.Event.GameType.Name));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _ticketRepository.GetTicketAsync(id.Value);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var model = new TicketsViewModel
            {
                Events = await _ticketRepository.GetComboEvents(),
                TicketTypes = new List<SelectListItem>
                {
                    new SelectListItem { Value = "Normal", Text = "Normal" },
                    new SelectListItem { Value = "Accessible", Text = "Accessible" }
                }
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TicketsViewModel model)
        {
            model.Events = await _ticketRepository.GetComboEvents();
            model.TicketTypes = new List<SelectListItem>
            {
                new SelectListItem { Value = "Normal", Text = "Normal" },
                new SelectListItem { Value = "Accessible", Text = "Accessible" }
            };

            if (ModelState.IsValid)
            {
                if (model.Price < 1)
                {
                    ModelState.AddModelError(string.Empty, "The Ticket price must be above 0");
                    return View(model);
                }

                var ticket = _converterHelper.ToTicket(model, true);

                if (await _ticketRepository.IsTicketAlreadyCreated(ticket))
                {
                    ModelState.AddModelError(string.Empty, "The Ticket already exits in the event especified");
                    return View(model);
                }

                await _ticketRepository.CreateAsync(ticket);
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

            var ticket = await _ticketRepository.GetTicketAsync(id.Value);
            if (ticket == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToTicketViewModel(ticket);
            model.Events = await _ticketRepository.GetComboEvents();
            model.TicketTypes = new List<SelectListItem>
            {
                new SelectListItem { Value = "Normal", Text = "Normal" },
                new SelectListItem { Value = "Accessible", Text = "Accessible" }
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TicketsViewModel model)
        {
            model.Events = await _ticketRepository.GetComboEvents();
            model.TicketTypes = new List<SelectListItem>
            {
                new SelectListItem { Value = "Normal", Text = "Normal" },
                new SelectListItem { Value = "Accessible", Text = "Accessible" }
            };

            if (ModelState.IsValid)
            {
                try
                {
                    var ticket = _converterHelper.ToTicket(model, false);

                    if (await _ticketRepository.IsTicketAlreadyCreated(ticket))
                    {
                        ModelState.AddModelError(string.Empty, "The Ticket already exits in the event especified");
                        return View(model);
                    }

                    await _ticketRepository.UpdateAsync(ticket);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _ticketRepository.ExistAsync(model.Id))
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

            var ticket = await _ticketRepository.GetTicketAsync(id.Value);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);

            await _ticketRepository.DeleteAsync(ticket);
            return RedirectToAction(nameof(Manage));
        }
    }
}
