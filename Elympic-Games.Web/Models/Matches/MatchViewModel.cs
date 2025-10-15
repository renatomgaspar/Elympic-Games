using Elympic_Games.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Elympic_Games.Web.Models.Matches
{
    public class MatchViewModel
    {
        public int Id { get; set; }


        [Required]
        [Display(Name = "Start Date and Time")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime StartDate { get; set; }


        [Required]
        [Display(Name = "End Date and Time")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime EndDate { get; set; }


        [Required]
        [Display(Name = "Team One")]
        public int TeamOneId { get; set; }


        [Required]
        [Display(Name = "Team Two")]
        public int TeamTwoId { get; set; }


        [Display(Name = "Team One Score")]
        public int? TeamOneScore { get; set; }


        [Display(Name = "Team Two Score")]
        public int? TeamTwoScore { get; set; }


        [Required]
        public int EventId { get; set; }


        [BindNever]
        public IEnumerable<SelectListItem>? Teams { get; set; }
    }
}
