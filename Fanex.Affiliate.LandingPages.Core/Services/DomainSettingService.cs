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

    public string GetCurrentMemberSiteHost(HttpRequest request)
    {
        var redirectLink = GetRootDomain(request);
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

    public string GetCurrentMemberSiteHost(HttpRequest request, string langCode)
    {
        var redirectLink = GetCurrentMemberSiteHost(request);
        return $"{redirectLink}/{langCode}";
    }

    public string GetMemberSiteLinkWithPath(HttpRequest request, string langCode, string path)
    {
        var host = GetCurrentMemberSiteHost(request, langCode).TrimEnd('/');
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

    public string GetCurrentLandingPageDomain(HttpRequest request)
    {
        var uriBuilder = new UriBuilder
        {
            Scheme = LandingPageConstants.HttpsScheme,
            Host = request.Host.Host,
            Port = request.Host.Port ?? LandingPageConstants.HttpsPort,
            Path = request.Path,
            Query = request.QueryString.HasValue ? request.QueryString.Value : string.Empty
        };
        return uriBuilder.Uri.ToString();
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

    public string GetUnderMaintenanceLink()
    {
        return options.Value.MemberSiteUM ?? string.Empty;
    }

    public bool GetEnableCheckUnderMaintenance()
    {
        return options.Value.EnableCheckMemberSiteUM;
    }

    public string GetTermsConditionsLink(string langCode)
    {
        var termsConditionsTpl = options.Value.TermsConditions;
        var termsConditions = string.Format(termsConditionsTpl, langCode);
        return termsConditions;
    }

    public string GetMediaRootDomain()
    {
        var mediaDomain = options.Value.MediaSite;
        return mediaDomain;
    }
}

