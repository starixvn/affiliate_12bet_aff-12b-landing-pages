using Fanex.Affiliate.LandingPages.Core;
using Fanex.Affiliate.LandingPages.Core.Models;
using Fanex.Affiliate.LandingPages.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fanex.Affiliate.LandingPages.Pages;

//[ResponseCache(Duration = 60, VaryByQueryKeys = new[] { "version" })]
public class LandingPageModel(ILandingPageService landingPageService, 
    ISignUpService signUpService) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public required string PageCode { get; set; }

    [BindProperty(SupportsGet = true)]
    public required string LanguageCode { get; set; }

    [BindProperty(SupportsGet = true)]
    public string Version { get; set; } = string.Empty;

    [BindProperty]
    public SignupRequest Signup { get; set; } = new SignupRequest();

    public LandingPage? LandingPage { get; set; }

    public async Task OnGetAsync()
    {
        LandingPage = await landingPageService.GetLandingPage(PageCode, false);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (Signup != null)
        {
            await signUpService.SentryErrorAsync(null, null);
            //_context.Customer.Add(Customer);
            //await _context.SaveChangesAsync();
        }
        return RedirectToPage("./Index");
    }


}
