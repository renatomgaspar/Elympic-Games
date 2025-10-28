using Elympic_Games.Mobile.Views;

namespace Elympic_Games.Mobile
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(ScanQrCodes), typeof(ScanQrCodes));
        }
    }
}
