using System.Globalization;
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
    public string? Version { get; set; }

    [BindProperty]
    public SignupRequest SignUp { get; set; } = new SignupRequest();

    public LandingPage? LandingPage { get; set; }

    [BindProperty(Name = "cf-turnstile-response")]
    public string? TurnstileResponse { get; set; }

    public async Task OnGetAsync()
    {
        var capchaInfo = await landingPageService.GetRecaptchaInfoAsync();
        LandingPage = await landingPageService.GetLandingPage(PageCode, false);
        SignUp.CaptchaType = capchaInfo.Data?.Type == 2 ? 2 : 0;
        LandingPage.IsTurnstileCaptchaEnabled = capchaInfo.IsTurnstileCaptchaEnabled;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            LandingPage = await landingPageService.GetLandingPage(PageCode, false);
            return Page();
        }

        if (SignUp != null)
        {
            if (SignUp.CaptchaType == 2)
            {
                SignUp.ReCaptchaToken = TurnstileResponse;
            }
            
            await signUpService.SentryErrorAsync(null, null);
            //_context.Customer.Add(Customer);
            //await _context.SaveChangesAsync();
        }
        return RedirectToPage("./Index");
    }


}
