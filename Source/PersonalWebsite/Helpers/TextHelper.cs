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

        public static string GetHttpErrorCodeTitle(int code)
        {
            return code switch
            {
                400 => "Bad Request",
                401 => "Unauthorized",
                403 => "Forbidden",
                404 => "Page Not Found",
                405 => "Method Not Allowed",
                500 => "Internal Server Error",
                503 => "Service Unavailable",
                _ => "Unknown Error"
            };
        }

        public static string GetHttpErrorCodeDescription(int code)
        {
            return code switch
            {
                400 => "The server could not understand the request.",
                401 => "Access is denied due to invalid credentials.",
                403 => "You don't have permission to access this resource.",
                404 => "The page you are looking for does not exist.",
                405 => "The method is not allowed for the requested URL.",
                500 => "An unexpected error occurred.",
                503 => "The server is currently unavailable.",
                _ => "An unexpected error has occurred."
            };
        }
    }
}
