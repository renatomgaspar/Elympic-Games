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
        [Display(Name = "Last Name*")]
        public string LastName { get; set; }
    }
}
