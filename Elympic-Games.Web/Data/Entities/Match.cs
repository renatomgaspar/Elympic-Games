using System.ComponentModel.DataAnnotations;

namespace Elympic_Games.Web.Data.Entities
{
    public class Match : IEntity
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
        public Team TeamOne { get; set; }


        [Required]
        [Display(Name = "Team Two")]
        public Team TeamTwo { get; set; }


        [Required]
        [Display(Name = "Team One Score")]
        public int? TeamOneScore { get; set; }


        [Required]
        [Display(Name = "Team Two Score")]
        public int? TeamTwoScore { get; set; }


        [Required]
        [Display(Name = "Game")]
        public GameType GameType { get; set; }

        [Required]
        public Event Event { get; set; }
    }
}
