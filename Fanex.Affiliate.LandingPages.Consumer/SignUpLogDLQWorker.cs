using EasyNetQ;
using EasyNetQ.Consumer;
using EasyNetQ.Topology;
using Fanex.Affiliate.LandingPages.Core;
using Fanex.Affiliate.LandingPages.Core.Models;
using Fanex.Affiliate.LandingPages.Core.Services.Interfaces;
using ONEbook.UM.Services;

namespace Fanex.Affiliate.LandingPages.Consumer;

public sealed class SignUpLogDLQWorker(
    IBus bus,
    IMaintenanceService maintenanceService,
    ITrackingSignUpService trackingSignUpService,
    IRedisService redisService,
    ILogService logService) : BackgroundService
{
    private IDisposable? consumerHandle;
    private bool isTurnOffConsumer;
    private readonly object maintenanceLock = new();
    private const int InitialDelayMilliseconds = 10;

    private Queue errorQueue;

    public void TurnOffConsumerMode()
    {
        lock (maintenanceLock)
        {
            if (isTurnOffConsumer)
            {
                return;
            }

            isTurnOffConsumer = true;
            consumerHandle?.Dispose();
            consumerHandle = null;
            logService.LogFileMessage("Maintenance mode enabled – message processing paused.");
        }
    }

    public void TurnOnConsumerMode()
    {
        lock (maintenanceLock)
        {
            if (!isTurnOffConsumer)
            {
                return;
            }

            isTurnOffConsumer = false;
            StartConsumer();
            logService.LogFileMessage("Maintenance mode disabled – message processing resumed.");
        }
    }

    public async Task CheckConsumerMode()
    {
        var isDisableConsumer = await IsTurnOffConsumer();
        if (isDisableConsumer)
        {
            TurnOffConsumerMode();
        }
        else
        {
            TurnOnConsumerMode();
        }
    }

    private bool IsCurrentlyUnderMaintenance()
    {
        var maintenanceTime = maintenanceService.GetUnderMaintenanceTime();
        return maintenanceTime.IsUnderMaintenanceTime;
    }

    private async Task<bool> IsTurnOffConsumer()
    {
        var isEnableConsumeFromCache = await redisService.GetAsync<bool>(Constants.TurnOnErrorQueueConsumer);
        return IsCurrentlyUnderMaintenance() || !isEnableConsumeFromCache;
    }

    private void StartConsumer()
    {
        consumerHandle = bus.Advanced.Consume<SignUpRequest>(errorQueue, async (message, _) =>
        {
            if (IsCurrentlyUnderMaintenance())
            {
                logService.LogFileMessage("Maintenance mode active – message retained in queue.");
                await Task.FromResult(AckStrategies.NackWithRequeue);
                return;
            }

            try
            {
                logService.LogFileMessage($"[Consumer] Processing: {message.Body.ToString()}");
                var result = await trackingSignUpService.WriteLogSignUpToDatabase(message.Body);
                if (!result.IsSuccessful)
                {
                    await logService.SentryErrorAsync($"WriteLogSignUpToDatabase fail - { result.ErrorMessage }");
                    await Task.FromResult(AckStrategies.NackWithRequeue);
                }
                else
                {
                    await Task.FromResult(AckStrategies.Ack);
                }
            }
            catch (Exception ex)
            {
                logService.LogFileMessage("Maintenance mode active – message retained in queue.");
                await logService.SentryErrorAsync(ex);
                await Task.FromResult(AckStrategies.NackWithRequeue);
            }
        });
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(InitialDelayMilliseconds, stoppingToken).ConfigureAwait(false);
        // Setup exchanges and queues
        var errorExchange = await bus.Advanced.ExchangeDeclareAsync(Constants.ErrorExchangeName, ExchangeType.Direct);
        errorQueue = await bus.Advanced.QueueDeclareAsync(Constants.ErrorQueueName, c => c
            .AsDurable(true)
            .AsExclusive(false)
            .AsAutoDelete(false)
            .WithArgument("x-queue-type", "quorum")
        );
        
        await bus.Advanced.BindAsync(errorExchange, errorQueue, Constants.RoutingKey);

        StartConsumer();

        while (!stoppingToken.IsCancellationRequested)
        {
            logService.LogFileMessage($"Worker running at: {DateTimeOffset.Now}");

            await CheckConsumerMode();

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken).ConfigureAwait(false);
        }
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        consumerHandle?.Dispose();
        return base.StopAsync(cancellationToken);
    }
}
