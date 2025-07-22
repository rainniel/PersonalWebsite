using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalWebsite.Constants;
using PersonalWebsite.Helpers;
using PersonalWebsite.Models;
using PersonalWebsite.Services;

namespace PersonalWebsite.Pages.Admin
{
    [Authorize]
    public class SettingsModel(IDataCacheService<SiteSetting> siteSetting) : PageModel
    {
        private readonly IDataCacheService<SiteSetting> _siteSetting = siteSetting;

        [BindProperty]
        public string WebsiteName { get; set; } = string.Empty;

        [BindProperty]
        public bool MaintenanceEnabled { get; set; }

        [BindProperty]
        public string MaintenanceMessage { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            WebsiteName = (await _siteSetting.GetLatestAsync(SettingNames.WebsiteName)).Value ?? "";
            MaintenanceEnabled = (await _siteSetting.GetLatestAsync(SettingNames.MaintenanceEnabled)).Value.ToBoolean();
            MaintenanceMessage = (await _siteSetting.GetLatestAsync(SettingNames.MaintenanceMessage)).Value ?? "";

            return Page();
        }

        public async Task<IActionResult> OnPostFormGeneral()
        {
            await _siteSetting.SaveAsync(SettingNames.WebsiteName, new SiteSetting(WebsiteName));

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostFormMaintenance()
        {
            await _siteSetting.SaveAsync(SettingNames.MaintenanceEnabled, new SiteSetting(MaintenanceEnabled.ToString()));
            await _siteSetting.SaveAsync(SettingNames.MaintenanceMessage, new SiteSetting(MaintenanceMessage));

            return Redirect($"{Routes.Admin.Settings}#maintenance");
        }
    }
}
