using System.ComponentModel.DataAnnotations;

namespace Elympic_Games.Web.Models.Accounts
{
    public class ActiveAccountViewModel
    {
        [Required]
        public string Id { get; set; }
    }
}
