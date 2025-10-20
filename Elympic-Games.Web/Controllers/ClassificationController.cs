using Elympic_Games.Web.Data;
using Elympic_Games.Web.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Elympic_Games.Web.Controllers
{
    public class ClassificationController : Controller
    {
        private readonly IEventRepository _eventRepository;
        private readonly IClassificationRepository _classificationRepository;

        public ClassificationController(
            IClassificationRepository classificationRepository,
            IEventRepository eventRepository)
        {
            _classificationRepository = classificationRepository;
            _eventRepository = eventRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GlobalClassification(int eventId)
        {
            var eventObj = await _eventRepository.GetEventAsync(eventId);

            if (eventObj == null)
            {
                return NotFound();
            }

            var classifications = await _classificationRepository.GetClassificationsForAllAsync(eventId);

            if (classifications == null || !classifications.Any())
            {
                return NotFound();
            }

            ViewBag.EventName = eventObj.Name;

            return View(classifications);
        }


        public async Task<IActionResult> ClassificationPerEvent(int eventId)
        {
            var eventObj = await _eventRepository.GetEventAsync(eventId);

            if (eventObj == null)
            {
                return NotFound();
            }

            var classifications = await _classificationRepository.GetClassificationsByEventIdAsync(eventId);

            ViewBag.EventName = eventObj.Name;
            ViewBag.EventGame = eventObj.GameType.Name;

            return View(classifications);
        }
    }
}
