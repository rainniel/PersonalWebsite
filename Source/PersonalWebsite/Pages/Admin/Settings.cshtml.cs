using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalWebsite.Constants;
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

        public async Task<IActionResult> OnGetAsync()
        {
            WebsiteName = (await _siteSetting.GetLatestAsync(SettingNames.WebsiteName)).Value ?? "";
            return Page();
        }

        public async Task<IActionResult> OnPostFormGeneral()
        {
            await _siteSetting.SaveAsync(SettingNames.WebsiteName, new SiteSetting(WebsiteName));
            return RedirectToPage();
        }
    }
}
