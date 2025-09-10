using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Fanex.Affiliate.LandingPages.Core.Models;

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
        "cf-visitor"
    ];

    public static IDictionary<string, string> BuildForwardHeaders(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        // Get MemberSiteOptions from DI
        var memberSiteOptions = context.RequestServices.GetService<IOptions<MemberSiteOptions>>();
        headers["ai-host"] = memberSiteOptions?.Value?.Host ?? string.Empty;

        // Add origin-host header
        headers["origin-host"] = $"{context.Request.Scheme}://{context.Request.Host}";

        // Forward specific headers if present
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
