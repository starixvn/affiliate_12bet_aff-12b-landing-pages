using EasyNetQ;
using Fanex.Affiliate.LandingPages.Consumer;
using Fanex.Affiliate.LandingPages.Core;
using Fanex.Affiliate.LandingPages.Core.Extensions;
using Fanex.Affiliate.LandingPages.Core.Redis;
using Fanex.Affiliate.LandingPages.Core.Services;
using Fanex.Affiliate.LandingPages.Core.Services.Interfaces;
using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.Logging;
using Fanex.Logging.Sentry.RavenClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using ONEbook.UM.DependencyInjection;
using ONEbook.UM.Services;
using SharpRaven;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<SignUpLogWorker>();
builder.Services.AddHostedService<SignUpLogDLQWorker>();
builder.Services.AddHostedService<SignUpLogRedisStorageWorker>();
builder.Services.AddHostedService<SyncLandingPageToCacheWorker>();

// Configure RabbitMQ
var rabbitMqConnectionString = builder.Configuration["RabbitMQConnectionString"];
if (!string.IsNullOrEmpty(rabbitMqConnectionString))
{
    builder.Services.AddSingleton(RabbitHutch.CreateBus(rabbitMqConnectionString));
}

builder.Services.AddHttpClient();
builder.Services.AddOneBookUmForService(builder.Configuration);
builder.Services.AddSingleton<IMaintenanceService, MaintenanceService>();
builder.Services.AddSingleton<IDynamicRepository, DynamicRepository>();
builder.Services.AddSingleton<ITrackingSignUpService, TrackingSignUpService>();
RedisConnectionHelper.Initialize(builder.Configuration);
builder.Services.AddSingleton<IConnectionMultiplexer>(sp => RedisConnectionHelper.Connection);
builder.Services.AddSingleton<IRedisService, RedisService>();
builder.Services.AddSingleton<ICacheKeyService, CacheKeyService>();
builder.Services.AddSingleton<ILandingPageService, LandingPageService>();
builder.Services.AddSingleton<Fanex.Logging.ILogger, Fanex.Logging.Logger>();
builder.Services.AddSingleton<ILogService, Fanex.Affiliate.LandingPages.Core.Services.LogService>();
builder.Services.AddSingleton<IRabbitBus, Fanex.Affiliate.LandingPages.Core.Services.RabbitBus>();

// Configure logging
var env = builder.Configuration["Sentry:Environment"];
var backEndDsn = builder.Configuration["Sentry:Dsn"];
var category = builder.Configuration["Sentry:LogCategory"];

if (!string.IsNullOrEmpty(backEndDsn) && !string.IsNullOrEmpty(category))
{
    LogManager.SetDefaultLogCategory(category).Use(new List<ILogEngine>
    {
        new SentryLogging(new SentryEngineOptions
        {
            Dsn = new Dsn(backEndDsn),
            Environment = env
        })
    });
}

DbSettingProviderManager
    .StartNewSession()
    .Register(builder.Configuration)
    .Run();

var host = builder.Build();

RouteToAction(host);

host.Run();

static void RouteToAction(WebApplication host)
{
    host.MapGet("/", () => "Affiliate.Link.Consumer running!");
    host.MapGet("/sentry", async () =>
    {
        var logService = host.Services.GetRequiredService<Fanex.Logging.ILogger>();
        await logService.ErrorAsync("Consumer Test Sentry");
        return "Consumer Test sentry!";
    });
    host.MapGet("/OnDQLQueueConsumer", async () =>
    {
        var redisService = host.Services.GetRequiredService<IRedisService>();
        var result = await redisService.SetAsync(Constants.TurnOnErrorQueueConsumer, true);
        return Results.Ok(result);
    });

    host.MapGet("/OffDQLQueueConsumer", async () =>
    {
        var redisService = host.Services.GetRequiredService<IRedisService>();
        var result = await redisService.SetAsync(Constants.TurnOnErrorQueueConsumer, false);
        return Results.Ok(result);
    });

    host.MapGet("/healthz", (HttpContext context) =>
    {
        var whoami = WhoamiHelper.GetContent(context);
        return Results.Text(whoami);
    });
}