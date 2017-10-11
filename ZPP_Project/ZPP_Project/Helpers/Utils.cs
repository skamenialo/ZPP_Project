using Microsoft.AspNet.Identity;
using System.Security.Cryptography;
using System.Text;

namespace ZPP_Project.Helpers
{
    public class SHA512PasswordHasher : PasswordHasher
    {
        public override PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            return hashedPassword.Equals(HashPassword(providedPassword)) ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }

        public override string HashPassword(string password)
        {
            byte[] data = Encoding.UTF8.GetBytes(password);
            byte[] result;
            using (SHA512 shaM = new SHA512Managed())
            {
                result = shaM.ComputeHash(data);
            }
            StringBuilder hex = new StringBuilder(result.Length * 2);
            foreach (byte b in result)
                hex.AppendFormat("{0:X2}", b);
            return hex.ToString();
        }
    }

    public class Roles
    {
        public const string ADMINISTRATOR = "Administrator";
        public const string STUDENT = "Student";
        public const string COMPANY = "Firma";
        public const string TEACHER = "Wykladowca";
    }

    public class Keys
    {
        public const string CURRENT_ROLE = "CURRENT_ROLE";
        public const string LOGIN_MODEL = "LOGIN_MODEL";
    }
}