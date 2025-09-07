using Fanex.Affiliate.LandingPages.Core.Models;

namespace Fanex.Affiliate.LandingPages.Core.Services.Interfaces;

public interface ILandingPageService
{
    Task<LandingPage> GetLandingPage(string pageCode, bool isPreview);
}
