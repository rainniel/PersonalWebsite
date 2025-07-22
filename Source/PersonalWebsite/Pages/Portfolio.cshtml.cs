using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalWebsite.Constants;
using PersonalWebsite.Models;
using PersonalWebsite.Services;

namespace PersonalWebsite.Pages
{
    public class PortfolioModel(IDataCacheService<PageSetting> pageSetting, IDataCacheService<PageContent> pageContent) : PageModel
    {
        private const string PageName = PageNames.Portfolio;
        private readonly IDataCacheService<PageSetting> _pageSetting = pageSetting;
        private readonly IDataCacheService<PageContent> _pageContent = pageContent;

        public string PageContent { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            if ((await _pageSetting.GetCachedAsync(PageName)).IsDisabled)
            {
                return NotFound();
            }

            PageContent = (await _pageContent.GetCachedAsync(PageName)).Content ?? "";
            return Page();
        }
    }
}
