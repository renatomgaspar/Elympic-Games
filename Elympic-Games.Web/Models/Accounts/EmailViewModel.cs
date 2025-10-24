using System.ComponentModel.DataAnnotations;

namespace Elympic_Games.Web.Models.Accounts
{
    public class EmailViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
