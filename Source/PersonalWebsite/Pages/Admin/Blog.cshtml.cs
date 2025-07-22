using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalWebsite.Constants;
using PersonalWebsite.Models;
using PersonalWebsite.Services;

namespace PersonalWebsite.Pages.Admin
{
    [Authorize]
    public class BlogModel(IDataCacheService<PageSetting> pageSetting) : PageModel
    {
        private const string PageName = PageNames.Blog;
        private readonly IDataCacheService<PageSetting> _pageSetting = pageSetting;

        [BindProperty]
        public bool DisablePage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            DisablePage = (await _pageSetting.GetLatestAsync(PageName)).IsDisabled;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _pageSetting.SaveAsync(PageName, new PageSetting(DisablePage));
            return RedirectToPage();
        }
    }
}
