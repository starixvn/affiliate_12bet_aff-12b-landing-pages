namespace Fanex.Affiliate.LandingPages.Core.Models;

public class LandingPage
{
    public int PageId { get; set; }

    public string PageCode { get; set; } = string.Empty;

    public string PageName { get; set; } = string.Empty;

    public int CampaignId { get; set; }

    public string TargetCountryCode { get; set; } = string.Empty;

    public string MarketId { get; set; } = string.Empty;

    public string DealIds { get; set; } = string.Empty;

    public int StatusId { get; set; }

    public bool IsTurnstileCaptchaEnabled { get; set; }

    public IEnumerable<LandingPageModule> Modules { get; set; }
}
