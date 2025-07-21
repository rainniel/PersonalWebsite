namespace PersonalWebsite.Helpers
{
    public static class PageHelper
    {
        public static string GetCurrentPageName(HttpContext context)
        {
            var path = context.Request.Path.Value;

            if (!string.IsNullOrEmpty(path))
            {
                if (path == "/")
                {
                    return "index";
                }
                else
                {
                    return path.Trim('/').Split('/').FirstOrDefault()?.ToLower() ?? "";
                }
            }

            return "";
        }
    }
}
