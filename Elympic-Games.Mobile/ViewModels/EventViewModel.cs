using CommunityToolkit.Mvvm.ComponentModel;
using Elympic_Games.Mobile.Models;
using Elympic_Games.Mobile.Services;
using System.Collections.ObjectModel;

namespace Elympic_Games.Mobile.ViewModels
{
    public partial class EventViewModel : ObservableObject
    {
        private readonly EventService _eventService;

        [ObservableProperty]
        private bool _isBusy;

        public ObservableCollection<Event> Events { get; } = new();

        public EventViewModel(EventService eventService)
        {
            _eventService = eventService;

            LoadEventsAsync();
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
    }
}
