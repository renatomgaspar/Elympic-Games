using Elympic_Games.Mobile.ViewModels;

namespace Elympic_Games.Mobile.Views;

[QueryProperty(nameof(EventId), "EventId")]
[QueryProperty(nameof(Mode), "Mode")]
public partial class ScanQrCodes : ContentPage
{
    private readonly ScanQrCodesViewModel _viewModel;

    public int EventId { get; set; }
    public int Mode { get; set; }
    
    public ScanQrCodes(ScanQrCodesViewModel viewModel)
	{
		InitializeComponent();

        _viewModel = viewModel;

        barcodeReader.Options = new ZXing.Net.Maui.BarcodeReaderOptions
        {
            Formats = ZXing.Net.Maui.BarcodeFormat.QrCode,
            AutoRotate = true,
            Multiple = false
        };
    }

    private async void barcodeReader_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
        var first = e.Results?.FirstOrDefault();
        if (first == null) return;

        barcodeReader.IsDetecting = false;

        await Dispatcher.DispatchAsync(async () =>
        {
            await _viewModel.ProcessQrCodeAsync(first.Value, EventId, Mode);

            barcodeReader.IsDetecting = true;
        });
    }
}