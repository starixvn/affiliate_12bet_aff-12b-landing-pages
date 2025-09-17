using System.Globalization;
using Fanex.Affiliate.LandingPages.Core;
using Fanex.Affiliate.LandingPages.Core.Extensions;
using Fanex.Affiliate.LandingPages.Core.Models;
using Fanex.Affiliate.LandingPages.Core.Redis;
using Fanex.Affiliate.LandingPages.Core.Services;
using Fanex.Affiliate.LandingPages.Core.Services.Interfaces;
using Fanex.Affiliate.Link.Workers;
using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.Logging;
using Fanex.Logging.Sentry.RavenClient;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using ONEbook.UM.DependencyInjection;
using SharpRaven;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages().AddViewLocalization()
    .AddDataAnnotationsLocalization();

builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<ForwardHeadersHandler>();
builder.Services.AddProxyWithForwardHeadersHttpClient(Constants.UseProxyWithForwardHeaders, builder.Configuration);
builder.Services.AddProxyHttpClient(Constants.UseProxy, builder.Configuration);
builder.Services.AddSingleton<IDynamicRepository, DynamicRepository>();
builder.Services.AddScoped<ISignUpService, SignUpService>();
builder.Services.AddScoped<ILandingPageService, LandingPageService>();
builder.Services.AddSingleton<ILogService, Fanex.Affiliate.LandingPages.Core.Services.LogService>();
builder.Services.AddSingleton<Fanex.Logging.ILogger, Logger>();

// Configure MemberSite options from appsettings.json
builder.Services.Configure<MemberSiteOptions>(builder.Configuration.GetSection("DomainSettings"));
builder.Services.AddSingleton<IMemberSiteConfigService, MemberSiteConfigService>();

RedisConnectionHelper.Initialize(builder.Configuration);
builder.Services.AddSingleton<IConnectionMultiplexer>(sp => RedisConnectionHelper.Connection);
builder.Services.AddSingleton<IRedisService, RedisService>();
builder.Services.AddSingleton<ICacheKeyService, CacheKeyService>();
builder.Services.AddScoped<ITrackingSignUpService, TrackingSignUpService>();

builder.Services.AddHostedService<RabbitMqBusHostedService>();
builder.Services.AddSingleton<IRabbitBus, RabbitBus>();

builder.Services.AddOneBookUmForService(builder.Configuration);
// Configure thread pool settings
const short DefaultWorkerThreads = 300;
const short DefaultCompletionPortThreads = 300;
_ = ThreadPool.SetMinThreads(DefaultWorkerThreads, DefaultCompletionPortThreads);

var env = builder.Configuration["Sentry:Environment"];
var backEndDsn = builder.Configuration["Sentry:Dsn"];
var category = builder.Configuration["Sentry:LogCategory"];
LogManager.SetDefaultLogCategory(category).Use(new List<ILogEngine>
        {
            new SentryLogging(new SentryEngineOptions
            {
                Dsn = new Dsn(backEndDsn),
                Environment = env
            })
        });

DbSettingProviderManager
    .StartNewSession()
    .Register(builder.Configuration)
    .Run();

RedisConnectionHelper.Initialize(builder.Configuration);
builder.Services.AddSingleton<IConnectionMultiplexer>(sp => RedisConnectionHelper.Connection);
const string dataProtectionKeys = "DataProtectionKeys";
var appName = builder.Configuration.GetValue<string>("fanex.caching.redis:ApplicationName");
builder.Services.AddDataProtection()
    .PersistKeysToStackExchangeRedis(RedisConnectionHelper.Connection, $"{appName}:{dataProtectionKeys}")
    .SetApplicationName("Aff.12B.LandingPage");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

var forwardedHeadersOptions = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedHost,
    RequireHeaderSymmetry = false
};

forwardedHeadersOptions.KnownNetworks.Clear();
forwardedHeadersOptions.KnownProxies.Clear();

app.UseForwardedHeaders(forwardedHeadersOptions);

app.UseHttpsRedirection();

var supportedCultures = new[] { "en-US", "vi-VN", "th-TH", "zh-CN", "te-IN" };
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList(),
    SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList()
};

localizationOptions.RequestCultureProviders = new[]
{
    new CustomRouteDataRequestCultureProvider()
};

app.Use((context, next) =>
{
    var contentSecurityPolicy = builder.Configuration.GetValue<string>("DomainSettings:ContentSecurityPolicy");
    context.Response.Headers.Remove("X-Frame-Options");
    context.Response.Headers["Content-Security-Policy"] = contentSecurityPolicy;

    return next();
});

app.UseRouting();
app.UseRequestLocalization(localizationOptions);
app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.MapControllers();

await app.RunAsync();