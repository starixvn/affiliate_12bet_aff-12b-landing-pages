using Fanex.Affiliate.LandingPages.Core.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Fanex.Affiliate.LandingPages.Core.Services;

public class MemberSiteConfigService(IConfiguration configuration) : IMemberSiteConfigService
{
    public string GetHost()
    {
        return configuration[ConfigKeys.MemberSite] ?? string.Empty;
    }

    public string GetAPIHost()
    {
        return configuration[ConfigKeys.API] ?? string.Empty;
    }

    public string GetFullLink(string path)
    {
        var host = GetHost().TrimEnd('/');
        var cleanPath = path.TrimStart('/');
        return $"{host}/{cleanPath}";
    }

    public string GetFullApiLink(string path)
    {
        var apiHost = GetAPIHost().TrimEnd('/');
        var cleanPath = path.TrimStart('/');
        return $"{apiHost}/{cleanPath}";
    }
}

