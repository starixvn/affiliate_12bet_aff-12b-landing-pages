using Fanex.Affiliate.LandingPages.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fanex.Affiliate.LandingPages.Pages;

public class FormTemplatecshtmlModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public required string PageCode { get; set; }

    [BindProperty(SupportsGet = true)]
    public required string LanguageCode { get; set; }

    public LandingPageModel? LandingPageModel { get; set; }

    public void OnGet()
    {
    }
}
