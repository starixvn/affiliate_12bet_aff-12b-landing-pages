using System.Reflection;
using System.Text;
using Fanex.Affiliate.LandingPages.Core.Models;
using Fanex.Affiliate.LandingPages.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Polly;

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

    [BindProperty(SupportsGet = true)]
    public string? AffClickId { get; set; }

    [BindProperty]
    public RegisterForm RegisterForm { get; set; } = new RegisterForm();

    public LandingPage? LandingPage { get; set; }

    public async Task OnGetAsync()
    {
        var capchaInfo = await signUpService.GetRecaptchaInfoAsync();
        LandingPage = await landingPageService.GetLandingPage(PageCode, false);
        RegisterForm.CaptchaType = capchaInfo.Data?.Type == 2 ? 2 : 0;
        LandingPage.IsTurnstileCaptchaEnabled = capchaInfo.IsTurnstileCaptchaEnabled;
        LandingPage.TurnstileSiteKey = capchaInfo!.Data!.SiteKey;
        RegisterForm.AreaCode = LandingPage.CountryInfo!.DialCode;
        RegisterForm.CurrencyId = LandingPage.CurrencyId;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid && RegisterForm != null)
        {
            RegisterForm.UserPwd = RegisterForm.Password;
            RegisterForm.UserLang = LanguageCode;

            var signUp = new SignUpRequest
            {
                Username = RegisterForm.Username,
                UserPwd = RegisterForm.Password,
                Email = RegisterForm.Email,
                UserLang = LanguageCode,
                PhoneNo = RegisterForm.PhoneNo,
                AreaCode = RegisterForm.AreaCode,
                CurrencyId = RegisterForm.CurrencyId,
                AffCode = RegisterForm.AffCode,
                CaptchaType = RegisterForm.CaptchaType,
                ReCaptchaToken = RegisterForm.ReCaptchaToken,
                AffClickId = AffClickId,
            };

            var result = await signUpService.SignUpAsync(signUp);
            if (result.ErrorCode == 0)
            {
                if (result.Cookies.Any())
                {
                    foreach (var cookie in result.Cookies)
                    {
                        Response.Headers.Append("Set-Cookie", cookie);
                    }
                }

                var rootUrl = $"https://stage.uatasia.com/en/";
                return Redirect(rootUrl);
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.Message);
            }
        }

        await OnGetAsync();
        return Page();
    }
}
