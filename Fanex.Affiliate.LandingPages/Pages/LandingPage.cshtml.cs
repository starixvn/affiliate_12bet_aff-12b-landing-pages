using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fanex.Affiliate.LandingPages.Pages;

[ResponseCache(Duration = 60, VaryByQueryKeys = new[] { "version" })]
public class LandingPageModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public required string LandingPageCode { get; set; }

    [BindProperty(SupportsGet = true)]
    public required string LanguageCode { get; set; }

    [BindProperty(SupportsGet = true)]
    public string Version { get; set; } = string.Empty;

    public void OnGet()
    {
    }
}
