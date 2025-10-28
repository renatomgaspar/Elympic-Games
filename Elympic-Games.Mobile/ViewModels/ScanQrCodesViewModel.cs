using CommunityToolkit.Mvvm.ComponentModel;
using Elympic_Games.Mobile.Services;
using System.Threading.Tasks;

namespace Elympic_Games.Mobile.ViewModels
{
    public class ScanQrCodesViewModel : ObservableObject
    {
        private readonly TicketService _ticketService;

        public ScanQrCodesViewModel(TicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public async Task ProcessQrCodeAsync(string ticketId, int eventId, int mode)
        {
            try
            {
                var availableTickets = await _ticketService.CheckTicketAvailable(ticketId, eventId, mode);

                if (availableTickets == 1)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Ticket Detected",
                        $"Ticket is valid",
                        "OK");
                }
                else if (availableTickets == 2)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Ticket Detected",
                        $"Ticket has already been scanned",
                        "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Ticket Detected",
                        $"Ticket is invalid or not from this event",
                        "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
