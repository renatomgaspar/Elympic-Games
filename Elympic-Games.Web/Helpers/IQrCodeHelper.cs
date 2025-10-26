namespace Elympic_Games.Web.Helpers
{
    public interface IQrCodeHelper
    {
        Task<byte[]> GenerateQrAsync(string content, int pixelsPerModule = 20);
    }
}
