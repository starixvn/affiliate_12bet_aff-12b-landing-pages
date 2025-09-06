using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Core.Models;

public class RegisterForm
{
    [Required(
   ErrorMessageResourceType = typeof(ValidationMessages),
   ErrorMessageResourceName = "RequiredField"
), StringLength(18, MinimumLength = 6)]
    public string UserName { get; set; } = string.Empty;
    [Required, StringLength(15, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;

    [EmailAddress]
    public string? Email { get; set; }

    public string PhoneNo { get; set; } = string.Empty;
    public string AreaCode { get; set; } = string.Empty;

    public int CurrencyId { get; set; }
    public string UserLang { get; set; } = string.Empty;
    public int CaptchaType { get; set; }
    public string ReCaptchaToken { get; set; } = string.Empty;
    public string AffClickId { get; set; } = string.Empty;
    public string AffMediaLinkId { get; set; } = string.Empty;
    public string SignupUrl { get; set; } = string.Empty;
    public string ReferralUrl { get; set; } = string.Empty;
    public string AffCode { get; set; } = string.Empty;

    // [Remote(action: "CheckUsername", controller: "Account")]
    [Remote(routeName: "/Account/Check", ErrorMessage = "Affiliate ID cant be found")]
    public string? AffId { get; set; }
}
