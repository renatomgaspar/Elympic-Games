using Elympic_Games.Web.Data;
using Elympic_Games.Web.Data.Entities;
using Elympic_Games.Web.Helpers;
using Elympic_Games.Web.Models.Countries;
using Elympic_Games.Web.Models.Gametypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Elympic_Games.Web.Controllers
{
    public class GametypeController : Controller
    {
        private readonly IGametypeRepository _gametypeRepository;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;

        public GametypeController(
            IGametypeRepository gametypeRepository,
            IBlobHelper blobHelper,
            IConverterHelper converterHelper)
        {
            _gametypeRepository = gametypeRepository;
            _blobHelper = blobHelper;
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
            return View(_gametypeRepository.GetAll().OrderBy(c => c.Name));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameType = await _gametypeRepository.GetByIdAsync(id.Value);
            if (gameType == null)
            {
                return NotFound();
            }

            return View(gameType);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GametypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "gametypes");
                }

                var gameType = _converterHelper.ToGametype(model, imageId, true);

                if (await _gametypeRepository.GametypeExistsByName(model.Name) != null)
                {
                    ModelState.AddModelError("Name", "This name is already in use.");
                    return View(model);
                }

                await _gametypeRepository.CreateAsync(gameType);
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

            var gameType = await _gametypeRepository.GetByIdAsync(id.Value);
            if (gameType == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToGametypeViewModel(gameType);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(GametypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Guid imageId = model.ImageId;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        await _blobHelper.DeleteBlobAsync(model.ImageId, "gametypes");
                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "gametypes");
                    }

                    var existingGametype = await _gametypeRepository.GametypeExistsByName(model.Name);
                    if (existingGametype != null && existingGametype.Id != model.Id)
                    {
                        ModelState.AddModelError("Name", "This name is already in use.");
                        return View(model);
                    }

                    var gameType = _converterHelper.ToGametype(model, imageId, false);

                    await _gametypeRepository.UpdateAsync(gameType);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _gametypeRepository.ExistAsync(model.Id))
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

            var gameType = await _gametypeRepository.GetByIdAsync(id.Value);
            if (gameType == null)
            {
                return NotFound();
            }

            return View(gameType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gameType = await _gametypeRepository.GetByIdAsync(id); ;

            if (gameType == null)
            {
                return NotFound();
            }

            if (await _gametypeRepository.HasDependenciesAsync(id))
            {
                ViewBag.ErrorTitle = $"{gameType.Name} can not be deleted!";
                ViewBag.ErrorMessage = $"The Game has already Events or Teams!";
                return View("Error");
            }

            if (gameType.ImageId != Guid.Empty)
            {
                await _blobHelper.DeleteBlobAsync(gameType.ImageId, "gametypes");
            }

            await _gametypeRepository.DeleteAsync(gameType);
            return RedirectToAction(nameof(Manage));
        }
    }
}
