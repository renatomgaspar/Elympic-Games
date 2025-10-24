using System.ComponentModel.DataAnnotations;

namespace Elympic_Games.Web.Models.Accounts
{
    public class RecoveryAccountViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [MinLength(6)]
        [Display(Name = "Password")]
        public string NewPassword { get; set; }


        [Required]
        [Compare("NewPassword")]
        [Display(Name = "Confirm Password")]
        public string Confirm { get; set; }
    }
}
