using System.Text.Json;
using Fanex.Affiliate.LandingPages.Core.Criteria;
using Fanex.Affiliate.LandingPages.Core.Models;
using Fanex.Affiliate.LandingPages.Core.Services.Interfaces;
using Fanex.Data.Repository;

namespace Fanex.Affiliate.LandingPages.Core.Services;

public class LandingPageService(IDynamicRepository dynamicRepository, 
    Logging.ILogger logger,
    IMemberSiteConfigService memberSiteConfigService,
    IHttpClientFactory httpClientFactory) : ILandingPageService
{
    public async Task SentryErrorAsync(string errMsg, Exception exception)
    {
        await logger.ErrorAsync(errMsg, exception);
    }
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

    public async Task<RecaptchaInfo> GetRecaptchaInfoAsync()
    {
        try
        {
            using var httpClient = httpClientFactory.CreateClient(Constants.UseProxyWithForwardHeaders);
            var request = new HttpRequestMessage(HttpMethod.Post, $"{memberSiteConfigService.GetAPIHost()}{ApiPath.GetRecaptchaInfo}");
            request.Headers.Add("accept", "text/plain");
           
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            
            var jsonContent = await response.Content.ReadAsStringAsync();
            var recaptchaInfo = JsonSerializer.Deserialize<RecaptchaInfo>(jsonContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            
            return recaptchaInfo ?? new RecaptchaInfo();
        }
        catch (Exception ex)
        {
            await SentryErrorAsync("Error getting RecaptchaInfo", ex);
            return new RecaptchaInfo();
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
