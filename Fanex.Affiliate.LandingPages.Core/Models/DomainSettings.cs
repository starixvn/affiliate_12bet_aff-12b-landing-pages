namespace Fanex.Affiliate.LandingPages.Core.Models;

public class DomainSettings
{
    public string MainMemberSite { get; set; } = string.Empty;

    public string API { get; set; } = string.Empty;

    public RedirectSettings Redirect { get; set; } = new RedirectSettings();
}

public class RedirectSettings
{
    public bool RedirectToRootDomain { get; set; }

    public string SpecificDomain { get; set; } = string.Empty;

    public string DefaultPath { get; set; } = string.Empty;
}
