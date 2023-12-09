namespace Physiosoft.Security
{
    public class EncryptionUtil
    {
        public static string Encrypt(string originalPassword)
        {
            var encryptedPassword = BCrypt.Net.BCrypt.HashPassword(originalPassword);
            return encryptedPassword;
        }

        public static bool IsValidPassword(string originalPassword, string encryptedPassword)
        {
            var isValid = BCrypt.Net.BCrypt.Verify(originalPassword, encryptedPassword);
            return isValid;
        }
    }
}
