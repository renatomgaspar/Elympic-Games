using Elympic_Games.Web.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Elympic_Games.Web.Controllers
{
    public class NewsController : Controller
    {
        private readonly IRssFeedHelper _rssFeedHelper;

        public NewsController(IRssFeedHelper rssFeedHelper)
        {
            _rssFeedHelper = rssFeedHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _rssFeedHelper.GetEsportsNewsAsync("https://esportsinsider.com/feed"));
        }
    }
}
