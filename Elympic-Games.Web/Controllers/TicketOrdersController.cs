using Elympic_Games.Web.Data;
using Elympic_Games.Web.Data.Entities;
using Elympic_Games.Web.Models.TicketOrders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elympic_Games.Web.Controllers
{
    public class TicketOrdersController : Controller
    {
        private readonly ITicketOrderRepository _ticketOrderRepository;
        private readonly ITicketRepository _ticketRepository;

        public TicketOrdersController(
            ITicketOrderRepository ticketOrderRepository,
            ITicketRepository ticketRepository)
        {
            _ticketOrderRepository = ticketOrderRepository;
            _ticketRepository = ticketRepository;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var model = await _ticketOrderRepository.GetTicketOrderAsync(this.User.Identity.Name);
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Cart()
        {
            var model = await _ticketOrderRepository.GetCartAsync(this.User.Identity.Name);
            return View(model);
        }

        [Authorize]
        public IActionResult AddTicket()
        {
            var model = new AddItemViewModel
            {
                Quantity = 1,
                Tickets = _ticketRepository.GetTicketsInEvent()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTicket(AddItemViewModel model)
        {
            model.Tickets = _ticketRepository.GetTicketsInEvent();

            if (ModelState.IsValid)
            {
                await _ticketOrderRepository.AddItemToOrderAsync(model, this.User.Identity.Name);
                return Redirect("Cart");
            }

            return View(model);
        }

        public async Task<IActionResult> DeleteItem(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _ticketOrderRepository.DeleteTicketFromCartAsync(id.Value);

            return RedirectToAction("Cart");
        }
    }
}
