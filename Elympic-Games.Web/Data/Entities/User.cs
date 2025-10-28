using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Elympic_Games.Web.Data.Entities
{
    public class User : IdentityUser
    {
        [Required]
        [Display(Name = "First Name*")]
        public string FirstName { get; set; }


        [Required]
        [Display(Name = "Last Names*")]
        public string LastName { get; set; }


        [Display(Name = "Image")]
        public Guid? ImageId { get; set; }


        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";


        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://localhost:7175/images/userDefaultImage.png"
            : $"https://elympicgames.blob.core.windows.net/users/{ImageId}";
    }
}
