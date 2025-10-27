using Elympic_Games.Web.Data.Entities;

namespace Elympic_Games.Web.Helpers
{
    public interface IPdfGeneratorHelper
    {
        Task<byte[]> FillPdflMultipleTickets(List<TicketOrderDetail> tickets);
    }
}
