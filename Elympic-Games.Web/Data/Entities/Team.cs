using System.ComponentModel.DataAnnotations;

namespace Elympic_Games.Web.Data.Entities
{
    public class Team : IEntity
    {
        public int Id { get; set; }


        [Required]
        public string Name { get; set; }


        [Required]
        public Country Country { get; set; }


        [Required]
        [Display(Name = "Game")]
        public GameType GameType { get; set; }


        [Required]
        [Display(Name = "Team Manager")]
        public User TeamManager { get; set; }
    }
}
