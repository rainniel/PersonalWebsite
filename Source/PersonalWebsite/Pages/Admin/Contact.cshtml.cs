using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalWebsite.Constants;
using PersonalWebsite.Models;
using PersonalWebsite.Services;

namespace PersonalWebsite.Pages.Admin
{
    [Authorize]
    public class ContactModel(IDataCacheService<PageSetting> pageSetting, IDataCacheService<SiteSetting> siteSetting) : PageModel
    {
        private const string PageName = PageNames.Contact;
        private readonly IDataCacheService<PageSetting> _pageSetting = pageSetting;
        private readonly IDataCacheService<SiteSetting> _siteSetting = siteSetting;

        [BindProperty]
        public bool DisablePage { get; set; }

        [BindProperty]
        public string? RecaptchaSiteKey { get; set; }

        [BindProperty]
        public string? RecaptchaSecretKey { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            DisablePage = (await _pageSetting.GetLatestAsync(PageName)).IsDisabled;
            RecaptchaSiteKey = (await _siteSetting.GetLatestAsync(SettingNames.RecaptchaSiteKey)).Value;
            RecaptchaSecretKey = (await _siteSetting.GetLatestAsync(SettingNames.RecaptchaSecretKey)).Value;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _pageSetting.SaveAsync(PageName, new PageSetting(DisablePage));
            await _siteSetting.SaveAsync(SettingNames.RecaptchaSiteKey, new SiteSetting(RecaptchaSiteKey));
            await _siteSetting.SaveAsync(SettingNames.RecaptchaSecretKey, new SiteSetting(RecaptchaSecretKey));
            return RedirectToPage();
        }
    }
}
