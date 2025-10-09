using Elympic_Games.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Elympic_Games.Web.Models.Cities
{
    public class CityViewModel
    {
        public int Id { get; set; }


        [Required]
        public string Name { get; set; }


        [Required]
        public int CountryId { get; set; }


        [BindNever]
        public IEnumerable<SelectListItem>? Countries { get; set; }
    }
}
