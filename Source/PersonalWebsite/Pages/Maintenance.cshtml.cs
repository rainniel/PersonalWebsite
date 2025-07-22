using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalWebsite.Constants;

namespace PersonalWebsite.Pages
{
    public class MaintenanceModel : PageModel
    {
        public string Message { get; set; } = "";

        public IActionResult OnGet()
        {
            if (!HttpContext.Items.ContainsKey(SettingNames.MaintenanceEnabled))
            {
                return NotFound();
            }

            if (HttpContext.Items.TryGetValue("MaintenanceMessage", out var message) && message is string msg)
            {
                Message = msg;
            }

            return Page();
        }
    }
}
