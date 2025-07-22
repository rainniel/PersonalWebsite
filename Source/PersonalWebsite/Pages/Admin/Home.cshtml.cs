using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalWebsite.Constants;
using PersonalWebsite.Models;
using PersonalWebsite.Services;

namespace PersonalWebsite.Pages.Admin
{
    [Authorize]
    public class HomeModel(IDataCacheService<PageContent> pageContent) : PageModel
    {
        private const string PageName = PageNames.Home;
        private readonly IDataCacheService<PageContent> _pageContent = pageContent;

        [BindProperty]
        public string ContentValue { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            ContentValue = (await _pageContent.GetLatestAsync(PageName)).Content ?? "";
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _pageContent.SaveAsync(PageName, new PageContent(ContentValue));
            return RedirectToPage();
        }
    }
}
