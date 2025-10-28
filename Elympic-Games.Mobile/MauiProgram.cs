using Elympic_Games.Mobile.Services;
using Elympic_Games.Mobile.ViewModels;
using Microsoft.Extensions.Logging;
using ZXing.Net.Maui.Controls;

namespace Elympic_Games.Mobile
{
    public static class MauiProgram
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .UseBarcodeReader();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddTransient<EventService>();

            builder.Services.AddTransient<EventViewModel>();

            var app = builder.Build();

            ServiceProvider = app.Services;

            return app;
        }
    }
}
