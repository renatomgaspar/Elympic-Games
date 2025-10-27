using System.Security.Cryptography;
using System.Text;

namespace Elympic_Games.Web.Helpers
{
    public class EncryptHelper : IEncryptHelper
    {
        private readonly string _passphrase;

        public EncryptHelper(IConfiguration configuration)
        {
            _passphrase = configuration["ElympicGamesAppKey"];

            if (string.IsNullOrWhiteSpace(_passphrase))
                throw new InvalidOperationException("The key 'ElympicGamesAppKey' was not found in the configuration or in the User Secrets.");
        }

        public string EncryptString(string message)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException(nameof(message));

            using var md5 = MD5.Create();
            using var tripleDes = TripleDES.Create();

            byte[] tdesKey = md5.ComputeHash(Encoding.UTF8.GetBytes(_passphrase));

            tripleDes.Key = tdesKey;
            tripleDes.Mode = CipherMode.ECB;
            tripleDes.Padding = PaddingMode.PKCS7;

            byte[] dataToEncrypt = Encoding.UTF8.GetBytes(message);

            using var encryptor = tripleDes.CreateEncryptor();
            byte[] results = encryptor.TransformFinalBlock(dataToEncrypt, 0, dataToEncrypt.Length);

            string enc = Convert.ToBase64String(results);
            enc = enc.Replace("+", "KOKOKO")
                     .Replace("/", "JOJOJO")
                     .Replace("\\", "IOIOIO");

            return enc;
        }

        public string DecryptString(string message)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException(nameof(message));

            using var md5 = MD5.Create();
            using var tripleDes = TripleDES.Create();

            byte[] tdesKey = md5.ComputeHash(Encoding.UTF8.GetBytes(_passphrase));

            tripleDes.Key = tdesKey;
            tripleDes.Mode = CipherMode.ECB;
            tripleDes.Padding = PaddingMode.PKCS7;

            message = message.Replace("KOKOKO", "+")
                             .Replace("JOJOJO", "/")
                             .Replace("IOIOIO", "\\");

            byte[] dataToDecrypt;

            try
            {
                dataToDecrypt = Convert.FromBase64String(message);
            }
            catch (Exception)
            {
                return "000000";
            }
            

            using var decryptor = tripleDes.CreateDecryptor();
            byte[] results = decryptor.TransformFinalBlock(dataToDecrypt, 0, dataToDecrypt.Length);

            return Encoding.UTF8.GetString(results);
        }
    }
}
