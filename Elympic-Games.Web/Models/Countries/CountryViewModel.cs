using Elympic_Games.Web.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Elympic_Games.Web.Models.Countries
{
    public class CountryViewModel : Country
    {
        [Display(Name = "Image")]
        public IFormFile? ImageFile { get; set; }
    }
}
