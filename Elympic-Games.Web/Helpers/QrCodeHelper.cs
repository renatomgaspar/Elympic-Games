using QRCoder;

namespace Elympic_Games.Web.Helpers
{
    public class QrCodeHelper : IQrCodeHelper
    {
        public Task<byte[]> GenerateQrAsync(string content, int pixelsPerModule = 20)
        {
            var qrGenerator = new QRCodeGenerator();
            var data = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
            var png = new PngByteQRCode(data);
            var bytes = png.GetGraphic(pixelsPerModule);
            return Task.FromResult(bytes);
        }
    }
}
