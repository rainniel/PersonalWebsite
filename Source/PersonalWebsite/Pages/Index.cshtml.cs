using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalWebsite.Models;
using PersonalWebsite.Services;

namespace PersonalWebsite.Pages
{
    public class IndexModel(IDBContent<PageContent> pageContent) : PageModel
    {
        private readonly IDBContent<PageContent> _pageContent = pageContent;
        public string PageContent { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            var content = await _pageContent.GetCachedAsync("Home");
            PageContent = content.Content;
        }
    }
}
