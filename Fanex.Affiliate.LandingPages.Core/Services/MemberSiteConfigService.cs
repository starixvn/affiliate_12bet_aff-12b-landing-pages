using Fanex.Affiliate.LandingPages.Core.Models;
using Fanex.Affiliate.LandingPages.Core.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace Fanex.Affiliate.LandingPages.Core.Services;

/// <summary>
/// Service for accessing MemberSite configuration values
/// </summary>
public class MemberSiteConfigService : IMemberSiteConfigService
{
    private readonly MemberSiteOptions _options;

    /// <summary>
    /// Initializes a new instance of the MemberSiteConfigService class
    /// </summary>
    /// <param name="options">The MemberSite options from configuration</param>
    public MemberSiteConfigService(IOptions<MemberSiteOptions> options)
    {
        _options = options.Value;
    }

    /// <summary>
    /// Gets the MemberSite host URL
    /// </summary>
    /// <returns>The host URL</returns>
    public string GetHost()
    {
        return _options.Host;
    }

    /// <summary>
    /// Gets the MemberSite API host URL
    /// </summary>
    /// <returns>The API host URL</returns>
    public string GetAPIHost()
    {
        return _options.APIHost;
    }

    /// <summary>
    /// Gets a complete URL by combining host with a path
    /// </summary>
    /// <param name="path">The path to append to the host</param>
    /// <returns>The complete URL</returns>
    public string GetFullUrl(string path)
    {
        var host = _options.Host.TrimEnd('/');
        var cleanPath = path.TrimStart('/');
        return $"{host}/{cleanPath}";
    }

    /// <summary>
    /// Gets a complete API URL by combining API host with a path
    /// </summary>
    /// <param name="path">The path to append to the API host</param>
    /// <returns>The complete API URL</returns>
    public string GetFullApiUrl(string path)
    {
        var apiHost = _options.APIHost.TrimEnd('/');
        var cleanPath = path.TrimStart('/');
        return $"{apiHost}/{cleanPath}";
    }
}
