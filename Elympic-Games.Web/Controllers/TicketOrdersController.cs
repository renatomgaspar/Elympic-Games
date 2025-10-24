using Elympic_Games.Web.Data;
using Elympic_Games.Web.Models.TicketOrders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

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

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketOrderDetails = await _ticketOrderRepository.GetTicketOrderDetails(id.Value);
            if (ticketOrderDetails == null)
            {
                return NotFound();
            }

            return View(ticketOrderDetails);
        }

        [Authorize]
        public IActionResult AddTicket()
        {
            var model = new AddItemViewModel
            {
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

        public async Task<IActionResult> ConfirmOrder()
        {
            var response = await _ticketOrderRepository.ConfirmOrderAsync(this.User.Identity.Name);

            if (response.Success)
            {
                TempData["Session"] = response.Message;
                return Redirect(response.StripeUrl);
            }

            ModelState.AddModelError(string.Empty, response.Message);
            return RedirectToAction("Cart");
        }

        public async Task<IActionResult> FinishOrder()
        {
            var service = new SessionService();
            Session session = service.Get(TempData["Session"].ToString());

            if (session.PaymentStatus == "paid")
            {
                var response = await _ticketOrderRepository.FinishOrder(this.User.Identity.Name);

                if (response.Success)
                {
                    return Redirect("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, response.Message);
                    return Redirect("Cart");
                }
                
            }

            return Redirect("Cart");
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
