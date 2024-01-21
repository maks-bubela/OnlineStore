using OnlineStore.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.BLL.Cryptography
{
    public class PasswordProcessing : IPasswordProcessing
    {
        public string GenerateSalt()
        {
            var bytes = new byte[16];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(bytes);
            }
            return Convert.ToBase64String(bytes);
        }

        public string GetHashCode(string pass, string salt)
        {
            if (String.IsNullOrWhiteSpace(pass)) throw new ArgumentException(nameof(pass));
            if (String.IsNullOrWhiteSpace(salt)) throw new ArgumentException(nameof(salt));

            var bytes = Encoding.UTF8.GetBytes(pass + salt);
            var sha256 = new SHA256Managed();
            var hashBytes = sha256.ComputeHash(bytes);

            return Convert.ToBase64String(hashBytes);
        }
    }
}
