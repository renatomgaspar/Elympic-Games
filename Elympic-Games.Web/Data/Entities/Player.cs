using System.ComponentModel.DataAnnotations;

namespace Elympic_Games.Web.Data.Entities
{
    public class Player : IEntity
    {
        public int Id { get; set; }


        [Required]
        [Display(Name = "First Name")]
        public string FírstName { get; set; }


        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        [Required]
        [Display(Name = "Birth Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }


        [Required]
        public Team Team { get; set; }


        [Required]
        [Display(Name = "Is Available")]
        public bool IsAvailable { get; set; }
    }
}
