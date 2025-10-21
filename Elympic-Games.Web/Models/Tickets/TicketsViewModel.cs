using Elympic_Games.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Elympic_Games.Web.Models.Tickets
{
    public class TicketsViewModel
    {
        public int Id { get; set; }


        [Required]
        public int EventId { get; set; }


        [Required]
        public string TicketType { get; set; }


        [Required]
        public decimal Price { get; set; }


        [BindNever]
        public IEnumerable<SelectListItem>? Events { get; set; }


        [BindNever]
        public IEnumerable<SelectListItem>? TicketTypes { get; set; }
    }
}
