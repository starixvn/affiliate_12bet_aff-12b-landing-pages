using System.Text.Json;
using Fanex.Affiliate.LandingPages.Core.Constants;
using Fanex.Affiliate.LandingPages.Core.Converters;
using Fanex.Affiliate.LandingPages.Core.Models;
using Fanex.Affiliate.LandingPages.Core.Services.Interfaces;

namespace Fanex.Affiliate.LandingPages.Core.Services;

public class UnderMaintenanceService(ILogService logService, 
    IDomainSettingService memberSiteConfigService,
    IRedisService redisService,
    ICacheKeyService cacheKeyService,
    IHttpClientFactory httpClientFactory) : IUnderMaintenanceService
{
    private const int cacheDurationInSeconds = 60;

    public async Task<ScheduleMaintenance> GetUnderMaintenanceInfoAsync()
    {
        var cacheKey = cacheKeyService.GetUnderMaintainInfoKey();
        var scheduleMaintenance = await redisService.GetAsync<ScheduleMaintenance>(cacheKey);
        scheduleMaintenance ??= await SyncUnderMaintenanceInfoAsync();
        return scheduleMaintenance;
    }

    public async Task<ScheduleMaintenance> SyncUnderMaintenanceInfoAsync()
    {
        var cacheKey = cacheKeyService.GetUnderMaintainInfoKey();
        var scheduleMaintenance = await GetMaintenanceInfoFromApiAsync();
        var slidingExpirationTime = TimeSpan.FromSeconds(cacheDurationInSeconds);
        await redisService.SetAsync(cacheKey, scheduleMaintenance, slidingExpirationTime);
        return scheduleMaintenance;
    }

    private async Task<ScheduleMaintenance> GetMaintenanceInfoFromApiAsync()
    {
        try
        {
            var underMaintenanceLink = memberSiteConfigService.GetUnderMaintenanceLink();
            using var httpClient = httpClientFactory.CreateClient(Settings.UseProxyWithForwardHeaders);
            var request = new HttpRequestMessage(HttpMethod.Get, underMaintenanceLink);
            request.Headers.Add("accept", "application/json");
            // Add cache-control header to ensure we get a fresh version, not from cache
            request.Headers.Add("Cache-Control", "no-cache");
            request.Headers.Add("Pragma", "no-cache");
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var jsonContent = await response.Content.ReadAsStringAsync();
            var scheduleMaintenance = JsonSerializer.Deserialize<ScheduleMaintenance>(jsonContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new DateTimeConverter(), new UnixTimestampConverter() }
            });

            return scheduleMaintenance ?? new ScheduleMaintenance();
        }
        catch (Exception ex)
        {
            await logService.SentryErrorAsync("Error getting MaintainInfo from API", ex);
            return new ScheduleMaintenance();
        }
    }

    public async Task<bool> GetIsUnderMaintenanceByDomainAsync(string domain)
    {
        var scheduleMaintenance = await GetUnderMaintenanceInfoAsync();

        if (scheduleMaintenance is null || !scheduleMaintenance.Maintain)
        {
            return false;
        }

        if (scheduleMaintenance.ExcludeDomain is null || scheduleMaintenance.ExcludeDomain.Contains(domain))
        {
            return false;
        }

        var nowUtcSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var isUnderMaintenance = nowUtcSeconds >= scheduleMaintenance.UTCTimeStart && nowUtcSeconds <= scheduleMaintenance.UTCTimeEnd;
        return isUnderMaintenance;
    }
}
