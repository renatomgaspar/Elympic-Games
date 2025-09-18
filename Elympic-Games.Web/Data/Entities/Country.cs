using System.ComponentModel.DataAnnotations;

namespace Elympic_Games.Web.Data.Entities
{
    public class Country : IEntity
    {
        public int Id { get; set; }


        [Required]
        [Display(Name = "Country Name")]
        public string Name { get; set; }


        [Display(Name = "Country Code")]
        [MaxLength(3)]
        public string Code { get; set; }
    }
}
