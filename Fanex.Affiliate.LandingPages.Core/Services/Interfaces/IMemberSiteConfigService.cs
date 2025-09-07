namespace Fanex.Affiliate.LandingPages.Core.Services.Interfaces;

/// <summary>
/// Service interface for accessing MemberSite configuration values
/// </summary>
public interface IMemberSiteConfigService
{
    /// <summary>
    /// Gets the MemberSite host URL
    /// </summary>
    /// <returns>The host URL</returns>
    string GetHost();

    /// <summary>
    /// Gets the MemberSite API host URL
    /// </summary>
    /// <returns>The API host URL</returns>
    string GetAPIHost();

    /// <summary>
    /// Gets a complete URL by combining host with a path
    /// </summary>
    /// <param name="path">The path to append to the host</param>
    /// <returns>The complete URL</returns>
    string GetFullUrl(string path);

    /// <summary>
    /// Gets a complete API URL by combining API host with a path
    /// </summary>
    /// <param name="path">The path to append to the API host</param>
    /// <returns>The complete API URL</returns>
    string GetFullApiUrl(string path);
}

