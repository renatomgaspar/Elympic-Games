using Elympic_Games.Web.Data.Entities;

namespace Elympic_Games.Web.Models.TicketOrders
{
    public class TicketOrderResult
    {
        public bool Success { get; set; }

        public string? Message { get; set; }

        public Event? FailedEvent { get; set; }

        public string StripeUrl { get; set; }
    }
}
