using Elympic_Games.Web.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Elympic_Games.Web.Models.Accounts
{
    public class ChangeUserViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }


        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Two Factor Enable")]
        public bool TwoFactor { get; set; }

        public Guid? ImageId { get; set; }


        [Display(Name = "Image")]
        public IFormFile? ImageFile { get; set; }


        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://localhost:44387/images/userDefaultImage.png"
            : $"https://elympicgames.blob.core.windows.net/users/{ImageId}";
    }
}
