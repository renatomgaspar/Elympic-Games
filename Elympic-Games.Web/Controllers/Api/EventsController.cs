using Elympic_Games.Web.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elympic_Games.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;

        public EventsController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        [HttpGet]
        public IActionResult GetEvents()
        {
            return Ok(_eventRepository.GetAll());
        }
    }
}
