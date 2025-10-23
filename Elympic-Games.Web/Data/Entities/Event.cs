using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Elympic_Games.Web.Data.Entities
{
    public class Event : IEntity
    {
        public int Id { get; set; }


        [Required]
        public string Name { get; set; }


        [Required]
        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }


        [Required]
        [Display(Name = "End Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }


        [Required]
        public int GameTypeId { get; set; }
        public GameType GameType { get; set; }


        [Required]
        public int ArenaId { get; set; }
        public Arena Arena { get; set; }


        [ValidateNever]
        public int AvailableSeats { get; set; }


        [ValidateNever]
        public int AvailableAccessibleSeats { get; set; }
    }
}
