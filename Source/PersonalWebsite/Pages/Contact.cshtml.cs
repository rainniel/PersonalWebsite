using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalWebsite.Constants;
using PersonalWebsite.Data;
using PersonalWebsite.Models;
using PersonalWebsite.Services;
using System.Text.Json;

namespace PersonalWebsite.Pages
{
    public class ContactModel(IDataCacheService<PageSetting> pageSetting, IDataCacheService<SiteSetting> siteSetting,
        IHttpClientFactory httpClientFactory, AppDbContext dbContext) : PageModel
    {
        private readonly IDataCacheService<PageSetting> _pageSetting = pageSetting;
        private readonly IDataCacheService<SiteSetting> _siteSetting = siteSetting;
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly AppDbContext _dbContext = dbContext;

        public bool EnableForm { get; set; }
        public string RecaptchaSiteKey { get; set; } = string.Empty;

        [BindProperty]
        public ContactMessage ContactMessage { get; set; } = default!;

        [TempData]
        public string StatusMessage { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            var pageDisabled = (await _pageSetting.GetCachedAsync(PageNames.Contact)).IsDisabled;
            var recaptchaSiteKey = (await _siteSetting.GetCachedAsync(SettingNames.RecaptchaSiteKey)).Value ?? "";
            var recaptchaSecretKey = (await _siteSetting.GetCachedAsync(SettingNames.RecaptchaSecretKey)).Value ?? "";

            if (!pageDisabled && !string.IsNullOrWhiteSpace(recaptchaSiteKey) && !string.IsNullOrWhiteSpace(recaptchaSecretKey))
            {
                EnableForm = true;
                RecaptchaSiteKey = recaptchaSiteKey;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var pageDisabled = (await _pageSetting.GetCachedAsync(PageNames.Contact)).IsDisabled;
            var recaptchaSiteKey = (await _siteSetting.GetCachedAsync(SettingNames.RecaptchaSiteKey)).Value ?? "";
            var recaptchaSecretKey = (await _siteSetting.GetCachedAsync(SettingNames.RecaptchaSecretKey)).Value ?? "";

            if (!pageDisabled && !string.IsNullOrWhiteSpace(recaptchaSiteKey) && !string.IsNullOrWhiteSpace(recaptchaSecretKey))
            {
                EnableForm = true;
                RecaptchaSiteKey = recaptchaSiteKey;

                if (!ModelState.IsValid)
                {
                    return Page();
                }

                var verificationPassed = false;
                var recaptchaResponse = Request.Form["g-recaptcha-response"].ToString();

                if (!string.IsNullOrWhiteSpace(recaptchaResponse))
                {
                    verificationPassed = await VerifyRecaptchaAsync(recaptchaSecretKey, recaptchaResponse);
                }

                if (verificationPassed)
                {
                    var forwardedIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                    var ipAddress = forwardedIp ?? HttpContext.Connection.RemoteIpAddress?.ToString();

                    ContactMessage.IPAddress = ipAddress;
                    ContactMessage.UserAgent = HttpContext.Request.Headers["User-Agent"].ToString();

                    _dbContext.ContactMessages.Add(ContactMessage);
                    await _dbContext.SaveChangesAsync();

                    StatusMessage = "Message submitted.";
                    return RedirectToPage();
                }
                else
                {
                    StatusMessage = "Captcha verification failed.";
                    return Page();
                }
            }

            return RedirectToPage();
        }

        private async Task<bool> VerifyRecaptchaAsync(string secret, string response)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                using var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("secret", secret),
                    new KeyValuePair<string, string>("response", response)
                });

                using var postResponse = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);
                var json = await postResponse.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<RecaptchaVerifyResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return result?.Success ?? false;
            }
            catch (Exception ex)
            {
                // IMPLEMENT LOGGING

                return false;
            }
        }
    }
}
