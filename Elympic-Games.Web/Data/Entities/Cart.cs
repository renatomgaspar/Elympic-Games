using System.ComponentModel.DataAnnotations;

namespace Elympic_Games.Web.Data.Entities
{
    public class Cart
    {
        public int Id { get; set; }


        [Required]
        public string UserId { get; set; }
        public User User { get; set; }


        [Required]
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }


        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Price { get; set; }
    }
}
