using System.Security.Cryptography;

namespace PersonalWebsite.Helpers
{
    public static class TextHelper
    {
        public static string GenerateRandomHash(int length)
        {
            if (length > 0)
            {
                var buffer = new byte[(int)Math.Ceiling(length / 2.0)];
                using var random = RandomNumberGenerator.Create();
                random.GetBytes(buffer);
                return Convert.ToHexString(buffer);
            }

            return string.Empty;
        }
    }
}
