using Elympic_Games.Web.Data;
using Elympic_Games.Web.Helpers;
using Elympic_Games.Web.Models.Matches;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Elympic_Games.Web.Controllers
{
    public class MatchController : Controller
    {
        private readonly IMatchRepository _matchRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IConverterHelper _converterHelper;

        public MatchController(
            IMatchRepository matchRepository,
            IEventRepository eventRepository,
            IConverterHelper converterHelper)
        {
            _matchRepository = matchRepository;
            _eventRepository = eventRepository;
            _converterHelper = converterHelper;
        }

        public async Task<IActionResult> Index(int eventId)
        {
            var eventObj = await _eventRepository.GetEventAsync(eventId);

            if (eventObj == null)
            {
                return NotFound();
            }

            var matches = await _matchRepository.GetMatchesByEventIdAsync(eventId);

            return View(matches);

        }

        [Authorize]
        [Authorize(Roles = "Admin")]
        public IActionResult Manage()
        {
            return View(_matchRepository
                .GetAll()
                .Include(m => m.TeamOne)
                .ThenInclude(t => t.Country)
                .Include(m => m.TeamTwo)
                .ThenInclude(t => t.Country)
                .Include(m => m.Event)
                .OrderBy(m => m.Event.Name)
                .ThenBy(m => m.StartDate));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _matchRepository.GetMatchAsync(id.Value);
            if (match == null)
            {
                return NotFound();
            }

            return View(match);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(int eventId)
        {
            var eventObj = await _eventRepository.GetEventAsync(eventId);

            if (eventObj == null)
            {
                return NotFound();
            }

            var model = new MatchViewModel
            {
                Teams = await _matchRepository.GetComboTeams(eventObj.GameTypeId),
                StartDate = eventObj.StartDate,
                EndDate = eventObj.EndDate,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MatchViewModel model)
        {
            var eventObj = await _eventRepository.GetEventAsync(model.EventId);

            if (eventObj == null)
            {
                return NotFound();
            }

            model.Teams = await _matchRepository.GetComboTeams(eventObj.GameTypeId);
            model.TeamOneScore = 0;
            model.TeamTwoScore = 0;

            if (ModelState.IsValid)
            {
                if (model.TeamOneId == model.TeamTwoId)
                {
                    ModelState.AddModelError(string.Empty, "Teams can not be the same");
                    return View(model);
                }

                if (model.StartDate < eventObj.StartDate || model.EndDate > eventObj.EndDate)
                {
                    ModelState.AddModelError(string.Empty, "Dates must be in the range of the event dates | " + eventObj.StartDate + " - " + eventObj.EndDate);
                    return View(model);
                }

                var match = _converterHelper.ToMatch(model, true);

                if (await _matchRepository.IsMaxMatchsAsync(match))
                {
                    ModelState.AddModelError(string.Empty, "You can not create more matches in this event. Insert results to progress and get the next matches");
                    return View(model);
                }

                if (await _matchRepository.ExistMatchInDatesAsync(match))
                {
                    ModelState.AddModelError(string.Empty, "There is already a match created in that range of dates");
                    return View(model);
                }

                var responseExitsMatchsByTeam = await _matchRepository.ExistMatchWithSameTeamsAsync(match);

                if (responseExitsMatchsByTeam == 1)
                {
                    ModelState.AddModelError(string.Empty, "Team 1 already have a match");
                    return View(model);
                }
                else if (responseExitsMatchsByTeam == 2)
                {
                    ModelState.AddModelError(string.Empty, "Team 2 already have a match");
                    return View(model);
                }

                await _matchRepository.CreateAsync(match);
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

            var match = await _matchRepository.GetMatchAsync(id.Value);
            if (match == null)
            {
                return NotFound();
            }

            var eventObj = await _eventRepository.GetEventAsync(match.EventId);

            if (eventObj == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToMatchViewModel(match);
            model.Teams = await _matchRepository.GetComboTeams(eventObj.GameTypeId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MatchViewModel model)
        {
            var eventObj = await _eventRepository.GetEventAsync(model.EventId);

            if (eventObj == null)
            {
                return NotFound();
            }

            model.Teams = await _matchRepository.GetComboTeams(eventObj.GameTypeId);

            if (ModelState.IsValid)
            {
                try
                {
                    if (model.TeamOneId == model.TeamTwoId)
                    {
                        ModelState.AddModelError(string.Empty, "Teams can not be the same");
                        return View(model);
                    }

                    if (model.StartDate < eventObj.StartDate || model.EndDate > eventObj.EndDate)
                    {
                        ModelState.AddModelError(string.Empty, "Dates must be in the range of the event dates | " + eventObj.StartDate + " - " + eventObj.EndDate);
                        return View(model);
                    }

                    var match = _converterHelper.ToMatch(model, false);

                    if (await _matchRepository.IsMaxMatchsAsync(match))
                    {
                        ModelState.AddModelError(string.Empty, "You can not create more matches in this event. Insert results to progress and get the next matches");
                        return View(model);
                    }

                    if (await _matchRepository.ExistMatchInDatesAsync(match))
                    {
                        ModelState.AddModelError(string.Empty, "There is already a match created in that range of dates");
                        return View(model);
                    }

                    var responseExitsMatchsByTeam = await _matchRepository.ExistMatchWithSameTeamsAsync(match);

                    if (responseExitsMatchsByTeam == 1)
                    {
                        ModelState.AddModelError(string.Empty, "Team 1 already have a match");
                        return View(model);
                    }
                    else if (responseExitsMatchsByTeam == 2)
                    {
                        ModelState.AddModelError(string.Empty, "Team 2 already have a match");
                        return View(model);
                    }

                    await _matchRepository.UpdateAsync(match);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _matchRepository.ExistAsync(model.Id))
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

            var match = await _matchRepository.GetMatchAsync(id.Value);
            if (match == null)
            {
                return NotFound();
            }

            return View(match);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var match = await _matchRepository.GetByIdAsync(id);

            await _matchRepository.DeleteAsync(match);
            return RedirectToAction(nameof(Manage));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> InsertScores(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _matchRepository.GetMatchAsync(id.Value);
            if (match == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToInsertScoresViewModel(match);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InsertScores(InsertScoresViewModel model)
        {
            if (model.Id == null)
            {
                return NotFound();
            }

            var match = await _matchRepository.GetMatchAsync(model.Id);
            if (match == null)
            {
                return NotFound();
            }

            model.TeamOne = match.TeamOne;
            model.TeamTwo = match.TeamTwo;

            if (ModelState.IsValid)
            {
                try
                {
                    if (model.TeamOneScore == model.TeamTwoScore)
                    {
                        ModelState.AddModelError(string.Empty, "The match can not have a draw");
                        return View(model);
                    }

                    match.TeamOneScore = model.TeamOneScore;
                    match.TeamTwoScore = model.TeamTwoScore;

                    await _matchRepository.UpdateAsync(match);
                    await _matchRepository.CreateNextMatch(match);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _matchRepository.ExistAsync(model.Id))
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
    }
}
