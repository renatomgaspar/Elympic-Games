using Elympic_Games.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Elympic_Games.Web.Models.Players
{
    public class PlayerViewModel
    {
        public int Id { get; set; }


        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }


        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        [Required]
        [Display(Name = "Birth Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }


        [Required]
        public int TeamId { get; set; }


        [Required]
        [Display(Name = "Is Playing")]
        public bool IsPlaying { get; set; }


        [BindNever]
        public IEnumerable<SelectListItem>? Teams { get; set; }
    }
}
