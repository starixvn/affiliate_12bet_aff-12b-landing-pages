using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Fanex.Affiliate.LandingPages.Core.Models;
using Fanex.Affiliate.LandingPages.Core.Constants;

namespace Fanex.Affiliate.LandingPages.Core.Extensions;
public static class HeaderForwardingHelper
{
    private static readonly string[] ForwardHeaderNames =
    [
        "x-forwarded-for",
        "x-forwarded-host",
        "x-client-ip",
        "x-actual-ip",
        "cloudfront-viewer-address",
        "cdn-loop",
        "cf-connecting-ip",
        "cf-ipcountry",
        "cf-ray",
        "cf-visitor",
        "User-Agent",
    ];

    public static IDictionary<string, string> BuildForwardHeaders(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var memberSiteOptions = context.RequestServices.GetService<IOptions<MemberSiteOptions>>();
        headers["ai-host"] = memberSiteOptions?.Value?.MemberSite ?? string.Empty;
        headers["origin-host"] = $"{LandingPageConstants.HttpsScheme}://{context.Request.Host}";

        foreach (var headerName in ForwardHeaderNames)
        {
            if (context.Request.Headers.TryGetValue(headerName, out var value) && !string.IsNullOrWhiteSpace(value))
            {
                headers[headerName] = value.ToString();
            }
        }

        return headers;
    }
}
