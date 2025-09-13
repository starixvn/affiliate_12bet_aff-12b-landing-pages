using Fanex.Affiliate.LandingPages.Core.Criteria;
using Fanex.Affiliate.LandingPages.Core.Models;
using Fanex.Affiliate.LandingPages.Core.Services.Interfaces;
using Fanex.Data.Repository;

namespace Fanex.Affiliate.LandingPages.Core.Services;

public class LandingPageService(IDynamicRepository dynamicRepository, IRedisService redisService) : ILandingPageService
{
    public async Task<LandingPage> GetLandingPage(string pageCode, bool isPreview)
    {
        const int cacheDurationInSeconds = 86400;
        var cacheKey = string.Format(CacheKey.LandingPage, isPreview ? 0 : 1, pageCode);
        var slidingExpirationTime = TimeSpan.FromSeconds(cacheDurationInSeconds);
        var landingPage = await redisService.GetAsync<LandingPage>(cacheKey);
        if (landingPage is null)
        {
            landingPage = await GetLandingPageFromDatabase(pageCode, isPreview);
            await redisService.SetAsync(cacheKey, landingPage, slidingExpirationTime);
        }

        return landingPage!;
    }

    public async Task<LandingPage> GetLandingPageFromDatabase(string pageCode, bool isPreview)
    {
        var criteria = new GetLandingPageCriteria
        {
            PageCode = pageCode,
            IsPreview = isPreview
        };

        var results = await dynamicRepository.FetchMultipleAsync<LandingPage, LandingPageModule>(criteria);
        var landingPage = results.Item1.FirstOrDefault();
        if (landingPage != null)
        {
            landingPage.Modules = results.Item2;
        }

        return landingPage!;
    }


    public async Task<IEnumerable<LandingPage>> GetAllLandingPages(bool isPreview)
    {
        var criteria = new GetAllLandingPageCriteria
        {
            IsPreview = isPreview
        };

        var results = await dynamicRepository.FetchMultipleAsync<LandingPage, LandingPageModule>(criteria);
        var landingPages = results.Item1;
        if (landingPages != null)
        {
            foreach(var landingPage in landingPages)
            {
                landingPage.Modules = results.Item2.Where(t=> t.PageId == landingPage.PageId);
            }
            
        }

        return landingPages!;
    }
}
