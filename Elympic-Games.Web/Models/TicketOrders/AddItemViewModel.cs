using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Elympic_Games.Web.Models.TicketOrders
{
    public class AddItemViewModel
    {
        [Display(Name = "Ticket")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a Ticket.")]
        public int TicketId { get; set; }


        [Range(1, int.MaxValue, ErrorMessage = "The quantity must be a positive number.")]
        public int Quantity { get; set; }


        [ValidateNever]
        public IEnumerable<SelectListItem> Tickets { get; set; }
    }
}
