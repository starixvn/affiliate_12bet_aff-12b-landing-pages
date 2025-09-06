using System.ComponentModel.DataAnnotations;
using System.Resources;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Core.Models;

public class SignupRequest
{
    //[Required, StringLength(18, MinimumLength = 6)]
    [Required(
       ErrorMessageResourceType = typeof(ValidationMessages),
       ErrorMessageResourceName = "RequiredField"
   ), StringLength(18, MinimumLength = 6)]
    public string UserName { get; set; } = string.Empty;
    [Required, StringLength(15, MinimumLength = 6)]
    public string UserPwd { get; set; } = string.Empty;

    [EmailAddress]
    public string? Email { get; set; }
    public string PhoneNo { get; set; } = string.Empty;
    public string AreaCode { get; set; } = string.Empty;
    public int CurrencyId { get; set; }
    public string UserLang { get; set; } = string.Empty;
    public int CaptchaType { get; set; }
    public string? ReCaptchaToken { get; set; }
    public string AffClickId { get; set; } = string.Empty;
    public string AffMediaLinkId { get; set; } = string.Empty;
    public string SignupUrl { get; set; } = string.Empty;
    public string ReferralUrl { get; set; } = string.Empty;
    public string AffCode { get; set; } = string.Empty;

    // [Remote(action: "CheckUsername", controller: "Account")]
    //[Remote(routeName: "/Account/Check", ErrorMessage = "Affiliate ID cant be found")]
    public string? AffId { get; set; }
    public string RefId { get; set; } = string.Empty;
    public string Aff { get; set; } = string.Empty;
    public string Blackbox { get; set; } = string.Empty;
    public string LineUserId { get; set; } = string.Empty;
    public string Nationality { get; set; } = string.Empty;
    public string RealName { get; set; } = string.Empty;
    public string PromoCode { get; set; } = string.Empty;
    public string RedirectUrl { get; set; } = string.Empty;
    public int Birth_d { get; set; }
    public int Birth_m { get; set; }
    public int Birth_y { get; set; }
    public string AccountNo { get; set; } = string.Empty;
    public int SysId { get; set; }
    public string IfscCode { get; set; } = string.Empty;
    public string JoinBy { get; set; } = string.Empty;
    public string JoinByBannerType { get; set; } = string.Empty;
    public string JiooToken { get; set; } = string.Empty;
}
