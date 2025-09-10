using Fanex.Affiliate.LandingPages.Core.Models;
using Fanex.Affiliate.LandingPages.Core.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace Fanex.Affiliate.LandingPages.Core.Services;

public class MemberSiteConfigService : IMemberSiteConfigService
{
    private readonly MemberSiteOptions _options;

    public MemberSiteConfigService(IOptions<MemberSiteOptions> options)
    {
        _options = options.Value;
    }

    public string GetHost()
    {
        return _options.Host;
    }

    public string GetAPIHost()
    {
        return _options.APIHost;
    }

    public string GetFullUrl(string path)
    {
        var host = _options.Host.TrimEnd('/');
        var cleanPath = path.TrimStart('/');
        return $"{host}/{cleanPath}";
    }

    public string GetFullApiUrl(string path)
    {
        var apiHost = _options.APIHost.TrimEnd('/');
        var cleanPath = path.TrimStart('/');
        return $"{apiHost}/{cleanPath}";
    }
}

