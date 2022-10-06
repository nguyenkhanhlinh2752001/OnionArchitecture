using System.Text;
using System.Security.Cryptography;

namespace Application.Helper
{
    public class EnCodeHelper
    {
        public static string EnCodeSha1(string pass)
        {
            var sha1 = new SHA1CryptoServiceProvider();

            var bs = sha1.ComputeHash(Encoding.UTF8.GetBytes(pass));

            var s = new StringBuilder();

            foreach (var b in bs)
            {
                s.Append(b.ToString("x1").ToLower());
            }

            return s.ToString();
        }
    }
}