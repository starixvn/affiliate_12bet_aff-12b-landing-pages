using Fanex.Affiliate.LandingPages.Core.Models;

namespace Fanex.Affiliate.LandingPages.Core.Services.Interfaces;

public interface ILandingPageService
{
    Task SentryErrorAsync(string errMsg, Exception exception);

    Task<LandingPage> GetLandingPage(string pageCode, bool isPreview);
}
