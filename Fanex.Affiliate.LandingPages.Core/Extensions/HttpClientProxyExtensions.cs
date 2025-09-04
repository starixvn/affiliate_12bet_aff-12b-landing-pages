using System.Net;
using Fanex.Affiliate.LandingPages.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fanex.Affiliate.LandingPages.Core.Extensions;

public static class HttpClientProxyExtensions
{
    public static IServiceCollection AddProxyHttpClient(
        this IServiceCollection services,
        string name,
        IConfiguration configuration)
    {
        var proxyOptions = new ProxyOptions();
        configuration.Bind($"HttpClients:{name}", proxyOptions);

        services.AddHttpClient(name)
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                if (!proxyOptions.IsUseProxy)
                {
                    return new HttpClientHandler();
                }

                var proxy = new WebProxy(proxyOptions.ProxyUrl)
                {
                    BypassProxyOnLocal = false,
                    UseDefaultCredentials = string.IsNullOrEmpty(proxyOptions.Username)
                };

                if (!string.IsNullOrEmpty(proxyOptions.Username) && proxyOptions.Password != null)
                {
                    proxy.Credentials = new NetworkCredential(proxyOptions.Username, proxyOptions.Password);
                }

                return new HttpClientHandler
                {
                    Proxy = proxy,
                    UseProxy = true
                };
            });

        return services;
    }
}
