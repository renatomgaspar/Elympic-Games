namespace Elympic_Games.Web.Helpers
{
    public interface IEncryptHelper
    {
        string EncryptString(string Message);

        string DecryptString(string Message);
    }
}
