using System.ComponentModel.DataAnnotations;

namespace Elympic_Games.Web.Data.Entities
{
    public class TicketOrder : IEntity
    {
        public int Id { get; set; }


        public IEnumerable<TicketOrderDetail> Items { get; set; }


        [Required]
        public User User { get; set; }
                

        [Required]
        [Display(Name = "Order Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime OrderDate { get; set; }


        [Required]
        public decimal TotalPrice { get; set; }
    }
}
