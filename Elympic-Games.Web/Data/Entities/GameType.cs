using System.ComponentModel.DataAnnotations;

namespace Elympic_Games.Web.Data.Entities
{
    public class GameType : IEntity
    {
        public int Id { get; set; }


        [Required]
        public string Name { get; set; }


        [Required]
        [Display(Name = "Team Size")]
        public int TeamSize { get; set; }


        [Required]
        [Display(Name = "Number of Players in Play")]
        public int ActivePlayerNo { get; set; } 
    }
}
