namespace Fanex.Affiliate.LandingPages.Core.Commands;

public class InsertSignUpLogCommand: BaseNonQueryCommand
{
    public string Username { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string PhoneNo { get; set; } = string.Empty;
    public string AreaCode { get; set; } = string.Empty;
    public int CurrencyId { get; set; }
    public string UserLang { get; set; } = string.Empty;
    public int CaptchaType { get; set; }
    public string ReCaptchaToken { get; set; } = string.Empty;
    public string? AffCode { get; set; }
    public string AffClickId { get; set; } = string.Empty;
    public string ReferralUrl { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;

    public long Duration { get; set; }

    public override string GetSettingKey() => "Aff_LandingPage_SignUp";

    public override bool IsValid() => true;
}
