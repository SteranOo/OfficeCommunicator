using System.Security.Cryptography;
using System.Text;

namespace OfficeCommunicator.Middleware.Utils
{
    public class Sha256StringEncoder : IStringEncoder
    {
        private readonly SHA256CryptoServiceProvider _provider = new SHA256CryptoServiceProvider();

        public string EncodeString(string str)
        {
            var data = Encoding.UTF8.GetBytes(str);
            var encodedData = _provider.ComputeHash(data);
            var encodedString = Encoding.UTF8.GetString(encodedData);
            return encodedString;
        }
    }
}
