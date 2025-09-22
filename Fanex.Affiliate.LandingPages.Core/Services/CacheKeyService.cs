using Fanex.Affiliate.LandingPages.Core.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Fanex.Affiliate.LandingPages.Core.Services;

public class CacheKeyService : ICacheKeyService
{
    private readonly string applicationName;

    public CacheKeyService(IConfiguration configuration)
    {
        applicationName = configuration["fanex.caching.redis:ApplicationName"] ?? "Aff.12B.LandingPage.Dev";
    }

    public string GetLandingPageKey(string pageCode)
    {
        return $"{applicationName}:{CacheKey.LandingPage}_{pageCode}";
    }

    public string GetLandingPagePreviewKey(string pageCode, int pageId)
    {
        return $"{applicationName}:{CacheKey.LandingPagePreview}_{pageCode}_{pageId}";
    }

    public string GetAllLandingPageVersionKey()
    {
        return $"{applicationName}:{CacheKey.AllLandingPageVersion}";
    }

    public string GetSignUpKey()
    {
        return $"{applicationName}:{CacheKey.SignUp}";
    }

    public string GetRecaptchaInfoKey(string langCode)
    {
        return $"{applicationName}:{CacheKey.RecaptchaInfo}_{langCode}";
    }

    public string GetUnderMaintainInfoKey()
    {
        return $"{applicationName}:{CacheKey.UnderMaintainInfo}";
    }
}

