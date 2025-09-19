using Fanex.Affiliate.LandingPages.Core.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Fanex.Affiliate.LandingPages.Core.Services;

public class CacheKeyService : ICacheKeyService
{
    private readonly string _applicationName;

    public CacheKeyService(IConfiguration configuration)
    {
        _applicationName = configuration["fanex.caching.redis:ApplicationName"] ?? "Aff.12B.LandingPage.Dev";
    }

    public string GetLandingPageKey(string pageCode)
    {
        return $"{_applicationName}:LandingPage_{pageCode}";
    }

    public string GetLandingPagePreviewKey(string pageCode, int pageId)
    {
        return $"{_applicationName}:LandingPagePreview_{pageCode}_{pageId}";
    }

    public string GetAllLandingPageVersionKey()
    {
        return $"{_applicationName}:AllLandingPageVersion";
    }

    public string GetSignUpKey()
    {
        return $"{_applicationName}:{CacheKey.SignUp}";
    }
}

