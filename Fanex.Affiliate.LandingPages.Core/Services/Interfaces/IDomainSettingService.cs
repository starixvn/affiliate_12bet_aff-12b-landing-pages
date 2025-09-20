using Microsoft.AspNetCore.Http;

namespace Fanex.Affiliate.LandingPages.Core.Services.Interfaces;

public interface IDomainSettingService
{
    string GetMainMemberSiteHost();

    string GetAPIHost();

    string GetCurrentMemberSiteHost(string rootDomain);

    string GetCurrentMemberSiteHost(string rootDomain, string langCode);

    string GetMemberSiteLinkWithPath(string rootDomain, string langCode, string path);

    string GetApiLink(string path);

    string GetRootDomain(HttpRequest request);

    string GetClientIpAddress(HttpContext httpContext);
}

