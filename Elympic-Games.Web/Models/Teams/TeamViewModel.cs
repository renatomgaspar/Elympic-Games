using Elympic_Games.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Elympic_Games.Web.Models.Teams
{
    public class TeamViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }


        [Required]
        [Display(Name = "Country")]
        public int CountryId { get; set; }


        [Required]
        [Display(Name = "Game")]
        public int GameTypeId { get; set; }


        [Required]
        [Display(Name = "Team Manager")]
        public string TeamManagerId { get; set; }


        [BindNever]
        public IEnumerable<SelectListItem>? GameTypes { get; set; }

        [BindNever]
        public IEnumerable<SelectListItem>? Countries { get; set; }

        [BindNever]
        public IEnumerable<SelectListItem>? Users { get; set; }
    }
}
