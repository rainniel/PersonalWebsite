namespace PersonalWebsite.Helpers
{
    public static class StringExtensions
    {
        public static bool ToBoolean(this string? str)
        {
            var value = str?.Trim();
            return value == "1" || string.Equals(value, "true", StringComparison.OrdinalIgnoreCase);
        }
    }
}
