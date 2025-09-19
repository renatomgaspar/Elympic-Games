using Elympic_Games.Web.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Elympic_Games.Web.Models
{
    public class ProductViewModel : Product
    {
        [Display(Name = "Image")]
        public IFormFile? ImageFile { get; set; }
    }
}
