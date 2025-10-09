using System.ComponentModel.DataAnnotations;

namespace Elympic_Games.Web.Data.Entities
{
    public class City : IEntity
    {
        public int Id { get; set; }


        [Required]
        public string Name { get; set; }


        [Required]
        public int CountryId { get; set; }
        public Country Country { get; set; }
    }
}
