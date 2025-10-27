using Elympic_Games.Web.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elympic_Games.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketOrderRepository _ticketOrderRepository;

        public TicketsController(ITicketOrderRepository ticketOrderRepository)
        {
            _ticketOrderRepository = ticketOrderRepository;
        }

        public ITicketOrderRepository TicketOrderRepository { get; }

        [HttpGet]
        [Route("checkticket")]
        public async Task<ActionResult<int>> CheckTicket([FromQuery] string ticketId, [FromQuery] int eventId)
        {
            return Ok(await _ticketOrderRepository.CheckTicket(ticketId, eventId));
        }
    }
}
