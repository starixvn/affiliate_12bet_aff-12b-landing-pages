namespace Fanex.Affiliate.LandingPages.Core.Models;

/// <summary>
/// Configuration options for MemberSite settings from appsettings.json
/// </summary>
public class MemberSiteOptions
{
    /// <summary>
    /// The host URL for the member site
    /// </summary>
    public string Host { get; set; } = string.Empty;

    /// <summary>
    /// The API host URL for the member site
    /// </summary>
    public string APIHost { get; set; } = string.Empty;
}
