using Elympic_Games.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Elympic_Games.Web.Models.Arenas
{
    public class ArenaViewModel
    {
        public int Id { get; set; }


        [Required]
        public string Name { get; set; }


        [Required]
        public int CityId { get; set; }


        [Required]
        [Display(Name = "Total Capacity")]
        public int TotalCapacity { get; set; }


        [Required]
        [Display(Name = "Number of Accessible Seating")]
        public int AccessibleSeating { get; set; }


        [BindNever]
        public IEnumerable<SelectListItem>? Cities { get; set; }
    }
}
