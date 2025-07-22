using PersonalWebsite.Constants;
using PersonalWebsite.Helpers;
using PersonalWebsite.Models;
using PersonalWebsite.Services;

namespace PersonalWebsite.Middleware
{
    public class MaintenanceMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;
        private static readonly string[] _staticFileExtensions = new[] { ".css", ".eot", ".ico", ".js", ".md", ".svg", ".ttf", ".txt", ".webp", ".woff" };

        public async Task InvokeAsync(HttpContext context, IDataCacheService<SiteSetting> siteSettings)
        {
            var path = context.Request.Path.ToString();
            if (path.StartsWith(Routes.Admin.Root, StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            if (_staticFileExtensions.Any(ext => path!.EndsWith(ext, StringComparison.OrdinalIgnoreCase)))
            {
                await _next(context);
                return;
            }

            var maintenanceEnabled = (await siteSettings.GetCachedAsync(SettingNames.MaintenanceEnabled)).Value.ToBoolean();
            if (maintenanceEnabled)
            {
                context.Items[SettingNames.MaintenanceEnabled] = true;
                context.Items[SettingNames.MaintenanceMessage] = (await siteSettings.GetCachedAsync(SettingNames.MaintenanceMessage))?.Value ?? "";

                context.Response.StatusCode = 503;
                context.Request.Path = Routes.Public.Maintenance;
                await _next(context);
                return;
            }
            else
            {
                await _next(context);
            }
        }
    }
}
