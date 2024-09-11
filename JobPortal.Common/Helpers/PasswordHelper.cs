using Microsoft.AspNetCore.Identity;

namespace JobPortal.Common.Helpers
{
    public class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            var passwordHasher = new PasswordHasher<object>();
            return passwordHasher.HashPassword(null, password);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            var passwordHasher = new PasswordHasher<object>();
            var verificationResult = passwordHasher.VerifyHashedPassword(null, hashedPassword, password);

            return verificationResult == PasswordVerificationResult.Success;
        }
    }
}
