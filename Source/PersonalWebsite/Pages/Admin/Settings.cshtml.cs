using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalWebsite.Constants;
using PersonalWebsite.Models;
using PersonalWebsite.Services;

namespace PersonalWebsite.Pages.Admin
{
    [Authorize]
    public class SettingsModel(IDBContent<SiteSetting> siteSetting) : PageModel
    {
        private readonly IDBContent<SiteSetting> _siteSetting = siteSetting;

        [BindProperty]
        public string WebsiteName { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            var siteSetting = await _siteSetting.GetLatestAsync(SettingNames.WebsiteName);
            WebsiteName = siteSetting.Value;
            return Page();
        }

        public async Task<IActionResult> OnPostFormGeneral()
        {
            await _siteSetting.SaveAsync(SettingNames.WebsiteName, WebsiteName);
            return RedirectToPage();
        }
    }
}
