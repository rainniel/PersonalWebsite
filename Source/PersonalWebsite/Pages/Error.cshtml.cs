using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace PersonalWebsite.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel(ILogger<ErrorModel> logger) : PageModel
    {
        private readonly ILogger<ErrorModel> _logger = logger;

        public int Code { get; set; }

        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public IActionResult OnGet(int? code)
        {
            var statusCodeFeature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();

            if (statusCodeFeature == null && exceptionFeature == null)
            {
                return NotFound();
            }

            Code = code ?? 500;
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            return Page();
        }
    }
}
