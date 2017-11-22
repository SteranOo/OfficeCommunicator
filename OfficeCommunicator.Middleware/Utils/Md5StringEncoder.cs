using System.Security.Cryptography;
using System.Text;

namespace OfficeCommunicator.Middleware.Utils
{
    public class Md5StringEncoder : IStringEncoder
    {
        private readonly MD5CryptoServiceProvider _provider = new MD5CryptoServiceProvider();

        public string EncodeString(string str)
        {
            var data = Encoding.UTF8.GetBytes(str);
            var encodedData = _provider.ComputeHash(data);
            var encodedString = Encoding.UTF8.GetString(encodedData);
            return encodedString;
        }
    }
}
