using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Elympic_Games.Mobile.Models;
using Elympic_Games.Mobile.Services;
using Elympic_Games.Mobile.Views;
using System.Collections.ObjectModel;

namespace Elympic_Games.Mobile.ViewModels
{
    public partial class EventViewModel : ObservableObject
    {
        private readonly EventService _eventService;

        [ObservableProperty]
        private bool _isBusy;

        public ObservableCollection<Event> Events { get; } = new();

        public IRelayCommand<Event> CheckInCommand { get; }
        public IRelayCommand<Event> CheckOutCommand { get; }

        public EventViewModel(EventService eventService)
        {
            _eventService = eventService;

            LoadEventsAsync();
            CheckInCommand = new RelayCommand<Event>(e => OnNavigateToScan(e, 1));
            CheckOutCommand = new RelayCommand<Event>(e => OnNavigateToScan(e, 0));
        }

        private async void LoadEventsAsync()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                var eventList = await _eventService.GetAllEvent();

                Events.Clear();
                foreach (var eventObj in eventList)
                    Events.Add(eventObj);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error loading events: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void OnNavigateToScan(Event selectedEvent, int mode)
        {
            if (selectedEvent == null)
                return;

            var parameters = new Dictionary<string, object>
            {
                { "EventId", selectedEvent.Id },
                { "Mode", mode }
            };

            await Shell.Current.GoToAsync(nameof(ScanQrCodes), parameters);
        }
    }
}
