using Elympic_Games.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Elympic_Games.Web.Models.Events
{
    public class EventViewModel
    {
        public int Id { get; set; }


        [Required]
        public string Name { get; set; }


        [Required]
        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }


        [Required]
        [Display(Name = "End Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }


        [Required]
        public int GameTypeId { get; set; }


        [Required]
        public int ArenaId { get; set; }


        [BindNever]
        public IEnumerable<SelectListItem>? GameTypes { get; set; }


        [BindNever]
        public IEnumerable<SelectListItem>? Arenas { get; set; }
    }
}
