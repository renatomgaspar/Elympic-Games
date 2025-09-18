using System.ComponentModel.DataAnnotations;

namespace Elympic_Games.Web.Data.Entities
{
    public class TicketOrderDetail : IEntity
    {
        public int Id { get; set; }


        [Required]
        public Ticket Ticket { get; set; }


        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal TotalPriceByDetail => TotalPriceByDetail * (decimal)Quantity;


        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double Quantity { get; set; }

    }
}
