using System.ComponentModel.DataAnnotations;
using Fanex.Affiliate.LandingPages.Core.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Core.Models;

public class RegisterForm
{
    [Required, StringLength(18, MinimumLength = 6)]
    [Remote(action: "CheckExistsUsername", controller: "SignUp", ErrorMessage = "Username is no longer available.")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "The Password field is required.")]
    [StringLength(15, MinimumLength = 6)]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d).{6,}$",
        ErrorMessage = "The field Password must be at least 6 characters, contain at least one letter and one number.")]
    public string UserPwd { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    [EmailAddress]
    [Remote(action: "CheckExistsEmail", controller: "SignUp", ErrorMessage = "Mail already exists.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "The Phone Number field is required.")]
    [PhoneNumberByCountry("CountryCode", ErrorMessage = "Invalid phone number for the selected country.")]
    public string PhoneNo { get; set; } = string.Empty;

    [Required]
    public string CountryCode { get; set; } = "VN"; // Default VN

    public string AreaCode { get; set; } = string.Empty;
    public int CurrencyId { get; set; }
    public string UserLang { get; set; } = string.Empty;
    public int CaptchaType { get; set; }

    public string ReCaptchaToken { get; set; } = string.Empty;
    public string AffClickId { get; set; } = string.Empty;
    public string AffMediaLinkId { get; set; } = string.Empty;
    public string SignupUrl { get; set; } = string.Empty;
    public string ReferralUrl { get; set; } = string.Empty;

    [Remote(action: "CheckAffiliate", controller: "SignUp")]
    public string? AffCode { get; set; }
    public string AffId { get; set; } = string.Empty;
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
