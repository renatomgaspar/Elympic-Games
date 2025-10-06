using Elympic_Games.Web.Data;
using Elympic_Games.Web.Data.Entities;
using Elympic_Games.Web.Helpers;
using Elympic_Games.Web.Models.Countries;
using Elympic_Games.Web.Models.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Elympic_Games.Web.Controllers
{
    public class CountryController : Controller
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;

        public CountryController(
            ICountryRepository countryRepository,
            IBlobHelper blobHelper,
            IConverterHelper converterHelper)
        {
            _countryRepository = countryRepository;
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
            return View(_countryRepository.GetAll().OrderBy(c => c.Name));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _countryRepository.GetByIdAsync(id.Value);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CountryViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Code = model.Code.ToUpper();

                Guid imageId = Guid.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "countries");
                }

                var country = _converterHelper.ToCountry(model, imageId, true);

                if (await _countryRepository.CountryExistsByCode(model.Code) != null)
                {
                    ModelState.AddModelError("Code", "This code is already in use.");
                    return View(model);
                }

                await _countryRepository.CreateAsync(country);
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

            var country = await _countryRepository.GetByIdAsync(id.Value);
            if (country == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToCountryViewModel(country);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CountryViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Code = model.Code.ToUpper();  

                try
                {
                    Guid imageId = model.ImageId;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        await _blobHelper.DeleteBlobAsync(model.ImageId, "countries");
                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "countries");
                    }

                    var existingCountry = await _countryRepository.CountryExistsByCode(model.Code);
                    if (existingCountry != null && existingCountry.Id != model.Id)
                    {
                        ModelState.AddModelError("Code", "This code is already in use.");
                        return View(model);
                    }

                    var country = _converterHelper.ToCountry(model, imageId, false);

                    await _countryRepository.UpdateAsync(country);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _countryRepository.ExistAsync(model.Id))
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

            var country = await _countryRepository.GetByIdAsync(id.Value);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var country = await _countryRepository.GetByIdAsync(id); ;

            if (country.ImageId != Guid.Empty)
            {
                await _blobHelper.DeleteBlobAsync(country.ImageId, "countries");
            }

            await _countryRepository.DeleteAsync(country);
            return RedirectToAction(nameof(Manage));
        }
    }
}
