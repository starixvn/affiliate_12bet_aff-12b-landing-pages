using Fanex.Affiliate.LandingPages.Core.Extensions;
using Fanex.Affiliate.LandingPages.Core.Redis;
using Fanex.Affiliate.LandingPages.Core.Services;
using Fanex.Affiliate.LandingPages.Core.Services.Interfaces;
using Fanex.Affiliate.LandingPages.Core.Models;
using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.Logging.Sentry.RavenClient;
using Fanex.Logging;
using SharpRaven;
using StackExchange.Redis;
using System.Reflection.Metadata;
using Fanex.Affiliate.LandingPages.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages().AddViewLocalization()
    .AddDataAnnotationsLocalization();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "en-US", "vi-VN" };
    options.SetDefaultCulture("en-US")
           .AddSupportedCultures(supportedCultures)
           .AddSupportedUICultures(supportedCultures);
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<ForwardHeadersHandler>();
builder.Services.AddProxyWithForwardHeadersHttpClient(Constants.UseProxyWithForwardHeaders, builder.Configuration);
builder.Services.AddProxyHttpClient(Constants.UseProxy, builder.Configuration);
builder.Services.AddSingleton<IDynamicRepository, DynamicRepository>();
builder.Services.AddScoped<ISignUpService, SignUpService>();
builder.Services.AddScoped<ILandingPageService, LandingPageService>();
builder.Services.AddSingleton<ILogService, Fanex.Affiliate.LandingPages.Core.Services.LogService>();
builder.Services.AddSingleton<Fanex.Logging.ILogger, Fanex.Logging.Logger>();

// Configure MemberSite options from appsettings.json
builder.Services.Configure<MemberSiteOptions>(builder.Configuration.GetSection("MemberSite"));
builder.Services.AddSingleton<IMemberSiteConfigService, MemberSiteConfigService>();

RedisConnectionHelper.Initialize(builder.Configuration);
builder.Services.AddSingleton<IConnectionMultiplexer>(sp => RedisConnectionHelper.Connection);
builder.Services.AddSingleton<IRedisService, RedisService>();

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.UseRequestLocalization();
app.MapRazorPages()
   .WithStaticAssets();

app.UseResponseCaching();

await app.RunAsync();
