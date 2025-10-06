using Elympic_Games.Web.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Elympic_Games.Web.Models.Gametypes
{
    public class GametypeViewModel : GameType
    {
        [Display(Name = "Image")]
        public IFormFile? ImageFile { get; set; }
    }
}
