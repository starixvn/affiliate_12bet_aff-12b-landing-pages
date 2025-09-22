using Fanex.Affiliate.LandingPages.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fanex.Affiliate.LandingPages.Pages;

public class FormTemplateModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public required string PageCode { get; set; }

    [BindProperty(SupportsGet = true)]
    public required string LanguageCode { get; set; }

  
    public string Username { get; set; } = string.Empty;

   
    public string UserPwd { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string PhoneNo { get; set; } = string.Empty;

    [BindProperty(SupportsGet = true)]
    public string CountryCode { get; set; } = string.Empty;

    public string AreaCode { get; set; } = string.Empty;
    public int CurrencyId { get; set; } 
    public string UserLang { get; set; } = "vn";
    public int CaptchaType { get; set; } = 2;

    public string ReCaptchaToken { get; set; } = string.Empty;
    public string AffClickId { get; set; } = string.Empty;
    public string AffMediaLinkId { get; set; } = string.Empty;
    public string SignUpUrl { get; set; } = string.Empty;
    public string ReferralUrl { get; set; } = string.Empty;

    public string? AffCode { get; set; }

    public string UserAgent { get; set; } = string.Empty;

    public string IpAddress { get; set; } = string.Empty;

    public string ApiUrl { get; set; } = string.Empty;

    public SignUpFormConfig? SignUpFormConfig { get; set; } 

    public string TermsConditionsURL { get; set; } = string.Empty;

    public bool IsTurnstileCaptchaEnabled { get; set; } = true;

    public string TurnstileSiteKey { get; set; } = string.Empty;


    public void OnGet()
    {
        SignUpFormConfig = SignUpFormConfigs.SupportSignUpForms
            .FirstOrDefault(t => t.FormCode == "F83");
    }
}
