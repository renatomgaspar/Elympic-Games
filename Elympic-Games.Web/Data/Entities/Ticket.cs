using System.ComponentModel.DataAnnotations;

namespace Elympic_Games.Web.Data.Entities
{
    public class Ticket : IEntity
    {
        public int Id { get; set; }


        [Required]
        public Event Event { get; set; }


        [Required]
        public int NoTicketEvent { get; set; }


        [Required]
        public string TicketType { get; set; }


        [Required]
        public decimal Price { get; set; }
    }
}
