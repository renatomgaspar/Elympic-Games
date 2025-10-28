using System.ComponentModel.DataAnnotations;

namespace Elympic_Games.Web.Data.Entities
{
    public class GameType : IEntity
    {
        public int Id { get; set; }


        [Required]
        public string Name { get; set; }


        [Required]
        [Range(1, 10, ErrorMessage = "Team size must be between 1 and 10.")]
        [Display(Name = "Team Size")]
        public int TeamSize { get; set; }


        [Required]
        [Range(1, 7, ErrorMessage = "Number of active players must be between 1 and 6.")]
        [Display(Name = "Number of Active Players")]
        public int ActivePlayerNo { get; set; }


        [Display(Name = "Game Image")]
        public Guid ImageId { get; set; }


        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://localhost:7175/images/noimage.png"
            : $"https://elympicgames.blob.core.windows.net/gametypes/{ImageId}";
    }
}
