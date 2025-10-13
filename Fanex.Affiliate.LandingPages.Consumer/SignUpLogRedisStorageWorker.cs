using Fanex.Affiliate.LandingPages.Core.Models;
using Fanex.Affiliate.LandingPages.Core.Services.Interfaces;
using ONEbook.UM.Services;

namespace Fanex.Affiliate.LandingPages.Consumer;

public sealed class SignUpLogRedisStorageWorker(
    IMaintenanceService maintenanceService,
    ITrackingSignUpService trackingSignUpService,
    IRedisService redisService,
    ICacheKeyService cacheKeyService,
    ILogProvider logService) : BackgroundService
{
    private const int JobDelayMinute = 1;
    private const int InitialDelayMilliseconds = 10;
    private bool isRunning;

    private bool IsCurrentlyUnderMaintenance()
    {
        var maintenanceTime = maintenanceService.GetUnderMaintenanceTime();
        return maintenanceTime.IsUnderMaintenanceTime;
    }

    private async Task StartConsumer()
    {
        if (isRunning || IsCurrentlyUnderMaintenance())
        {
            return;
        }

        isRunning = true;
        try
        {
            while (true)
            {
                var signUpRequest = await redisService.ListLeftPopAsync<SignUpRequest>(cacheKeyService.GetSignUpKey()).ConfigureAwait(false);
                if (signUpRequest == null)
                {
                    break;
                }

                var result = await trackingSignUpService.WriteLogSignUpToDatabase(signUpRequest).ConfigureAwait(false);
                if (!result.IsSuccessful)
                {
                    await logService.SentryErrorAsync("WriteLogSignUpToDatabase failed", new Exception(result.ErrorMessage)).ConfigureAwait(false);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            await logService.SentryErrorAsync(ex).ConfigureAwait(false);
        }
        finally
        {
            isRunning = false;
        }
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(InitialDelayMilliseconds, stoppingToken).ConfigureAwait(false);

        while (!stoppingToken.IsCancellationRequested)
        {
            logService.LogFileMessage($"SignUpLogRedisStorageWorker running at: {DateTimeOffset.Now}");
            await StartConsumer();
            await Task.Delay(TimeSpan.FromMinutes(JobDelayMinute), stoppingToken).ConfigureAwait(false);
        }
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        return base.StopAsync(cancellationToken);
    }
}
