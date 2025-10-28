using CommunityToolkit.Mvvm.ComponentModel;
using Elympic_Games.Mobile.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Elympic_Games.Mobile.ViewModels
{
    public class ScanQrCodesViewModel : ObservableObject
    {
        private readonly TicketService _ticketService;

        public ICommand BarcodeDetectedCommand { get; }

        public ScanQrCodesViewModel(TicketService ticketService)
        {
            _ticketService = ticketService;

            // Recebe os dois parâmetros: ticketId e eventId
            BarcodeDetectedCommand = new Command<(string ticketId, int eventId, int mode)>(async param =>
            {
                var (ticketId, eventId, mode) = param;
                await ProcessQrCodeAsync(ticketId, eventId, mode);
            });
        }

        private async Task ProcessQrCodeAsync(string ticketId, int eventId, int mode)
        {
            try
            {
                var availableTickets = await _ticketService.CheckTicketAvailable(ticketId, eventId, mode);

                if (availableTickets == 1)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Ticket Detected",
                        $"Ticket is valid. The person can enter the event",
                        "OK");
                }
                if (availableTickets == 2)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Ticket Detected",
                        $"Ticket is valid. The person can leave the event",
                        "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Ticket Detected",
                        $"Ticket is not from this event or is invalid",
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
