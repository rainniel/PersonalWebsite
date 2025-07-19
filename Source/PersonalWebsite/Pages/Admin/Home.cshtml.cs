using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalWebsite.Models;
using PersonalWebsite.Services;

namespace PersonalWebsite.Pages.Admin
{
    [Authorize]
    public class HomeModel(IDBContent<PageContent> pageContent) : PageModel
    {
        private const string PageName = "Home";
        private readonly IDBContent<PageContent> _pageContent = pageContent;

        [BindProperty]
        public string PageContent { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            var pageContent = await _pageContent.GetLatestAsync(PageName);
            PageContent = pageContent.Content;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _pageContent.SaveAsync(PageName, PageContent);
            return RedirectToPage();
        }
    }
}
