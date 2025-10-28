using Elympic_Games.Mobile.ViewModels;

namespace Elympic_Games.Mobile.Views;

public partial class EventsPage : ContentPage
{
	public EventsPage(EventViewModel viewModel)
	{
		InitializeComponent();
        
        BindingContext = viewModel;
        Shell.SetNavBarIsVisible(this, false);
    }
}