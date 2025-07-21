using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalWebsite.Models;
using PersonalWebsite.Services;

namespace PersonalWebsite.Pages
{
    public class IndexModel(IDataCacheService<PageContent> pageContent) : PageModel
    {
        private readonly IDataCacheService<PageContent> _pageContent = pageContent;
        public string PageContent { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            PageContent = (await _pageContent.GetCachedAsync("Home")).Content ?? "";
        }
    }
}
