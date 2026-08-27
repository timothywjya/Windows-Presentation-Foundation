using System.Security.Cryptography;
using System.Text;
using Belajar_1.Domain.Interfaces;

namespace Belajar_1.Infrastructure.Security
{
    public class Md5PasswordHasher : IPasswordHasher
    {
        public string Hash(string plainText)
        {
            using MD5 md5 = MD5.Create();
            byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(plainText));

            var sb = new StringBuilder();
            foreach (byte b in hashBytes)
                sb.Append(b.ToString("x2"));

            return sb.ToString();
        }
    }
}
