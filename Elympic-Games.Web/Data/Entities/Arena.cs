using System.ComponentModel.DataAnnotations;

namespace Elympic_Games.Web.Data.Entities
{
    public class Arena : IEntity
    {
        public int Id { get; set; }


        [Required]
        public string Name { get; set; }


        [Required]
        public City City { get; set; }


        [Required]
        [Display(Name = "Total Capacity")]
        public int TotalCapacity { get; set; }


        [Required]
        [Display(Name = "Number of Accesible Seating")]
        public int AccessibleSeating { get; set; }
    }
}
