using System.ComponentModel.DataAnnotations;

namespace Elympic_Games.Web.Data.Entities
{
    public class Country : IEntity
    {
        public int Id { get; set; }


        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }


        [Required]
        [Display(Name = "Code")]
        [MaxLength(3)]
        public string Code { get; set; }


        [Display(Name = "Flag")]
        public Guid ImageId { get; set; }


        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://localhost:44387/images/noimage.png"
            : $"https://elympicgames.blob.core.windows.net/countries/{ImageId}";
    }
}
