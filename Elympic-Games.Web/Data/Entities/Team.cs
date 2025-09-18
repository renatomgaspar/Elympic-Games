using System.ComponentModel.DataAnnotations;

namespace Elympic_Games.Web.Data.Entities
{
    public class Team : IEntity
    {
        public int Id { get; set; }


        [Required]
        public string Name { get; set; }


        [Required]
        public string Country { get; set; }


        [Required]
        [Display(Name = "Game Type")]
        public GameType GameType { get; set; }
    }
}
