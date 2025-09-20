using Fanex.Affiliate.LandingPages.Core.Constants;
using Fanex.Affiliate.LandingPages.Core.Models;
using Fanex.Affiliate.LandingPages.Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Fanex.Affiliate.LandingPages.Core.Services;

public class DomainSettingService(IOptions<DomainSettings> options) : IDomainSettingService
{
    public string GetMainMemberSiteHost()
    {
        return options.Value.MainMemberSite ?? string.Empty;
    }

    public string GetAPIHost()
    {
        return options.Value.API ?? string.Empty;
    }

    public string GetCurrentMemberSiteHost(string rootDomain)
    {
        var redirectLink = rootDomain;
        if (!options.Value.Redirect.RedirectToRootDomain)
        {
            var specificDomain = options.Value.Redirect.SpecificDomain;
            if (!string.IsNullOrEmpty(specificDomain))
            {
                redirectLink = specificDomain;
            }
        }

        return redirectLink;
    }

    public string GetCurrentMemberSiteHost(string rootDomain, string langCode)
    {
        var redirectLink = GetCurrentMemberSiteHost(rootDomain);
        return $"{redirectLink}/{langCode}";
    }

    public string GetMemberSiteLinkWithPath(string rootDomain, string langCode, string path)
    {
        var host = GetCurrentMemberSiteHost(rootDomain, langCode).TrimEnd('/');
        var cleanPath = path.TrimStart('/');
        return $"{host}/{cleanPath}";
    }

    public string GetApiLink(string path)
    {
        var apiHost = GetAPIHost().TrimEnd('/');
        var cleanPath = path.TrimStart('/');
        return $"{apiHost}/{cleanPath}";
    }

    public string GetRootDomain(HttpRequest request)
    {
        var host = request.Host.Host;
        var parts = host.Split('.', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length >= LandingPageConstants.MinimumDomainParts)
        {
            var rootDomain = string.Join(".", parts[^2], parts[^1]);
            return $"{LandingPageConstants.HttpsScheme}://{rootDomain}";
        }

        return $"{LandingPageConstants.HttpsScheme}://{host}";
    }

    public string GetClientIpAddress(HttpContext httpContext)
    {
        var realIp = httpContext.Request.Headers["x-actual-ip"].FirstOrDefault();
        if (!string.IsNullOrEmpty(realIp))
        {
            return realIp;
        }
        else
        {
            var forwardedFor = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwardedFor))
            {
                return forwardedFor.Split(',')[0].Trim();
            }
        }

        return httpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
    }
}

