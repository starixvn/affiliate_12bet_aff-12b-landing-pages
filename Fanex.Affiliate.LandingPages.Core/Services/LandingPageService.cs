using Fanex.Affiliate.LandingPages.Core.Criteria;
using Fanex.Affiliate.LandingPages.Core.Models;
using Fanex.Affiliate.LandingPages.Core.Services.Interfaces;
using Fanex.Data.Repository;

namespace Fanex.Affiliate.LandingPages.Core.Services;

public class LandingPageService(IDynamicRepository dynamicRepository) : ILandingPageService
{
    public async Task<LandingPage> GetLandingPage(string pageCode, bool isPreview)
    {
        var criteria = new GetLandingPageCriteria
        {
            PageCode = pageCode,
            IsPreview = isPreview
        };
        try
        {
            var results = await dynamicRepository.FetchMultipleAsync<LandingPage, LandingPageModule>(criteria);
            var landingPage = results.Item1.FirstOrDefault();
            if (landingPage != null)
            {
                landingPage.Modules = results.Item2;
            }
            else
            {
                landingPage = GetFakeLandingPage(criteria.PageCode);
            }

            return landingPage;
        }
        catch (Exception)
        {
            return GetFakeLandingPage(criteria.PageCode);
        }
    }

    private LandingPage GetFakeLandingPage(string pageCode)
    {
        var landingPage = new LandingPage
        {
            CampaignId = 1,
            PageCode = pageCode,
            Modules = new List<LandingPageModule>
                {
                    new LandingPageModule
                    {
                        ModuleId = 1
                    },
                    new LandingPageModule
                    {
                        ModuleId = 2
                    },
                    new LandingPageModule
                    {
                        ModuleId = 4
                    },
                    new LandingPageModule
                    {
                        ModuleId = 5
                    },
                    new LandingPageModule
                    {
                        ModuleId = 6
                    },
                    new LandingPageModule
                    {
                        ModuleId = 7
                    },
                    new LandingPageModule
                    {
                        ModuleId = 8
                    },
                    new LandingPageModule
                    {
                        ModuleId = 9
                    },
                    new LandingPageModule
                    {
                        ModuleId = 10
                    },
                    new LandingPageModule
                    {
                        ModuleId = 11
                    },
                    new LandingPageModule
                    {
                        ModuleId = 12
                    },
                    new LandingPageModule
                    {
                        ModuleId = 13
                    },
                }
        };

        return landingPage;
    }
}
