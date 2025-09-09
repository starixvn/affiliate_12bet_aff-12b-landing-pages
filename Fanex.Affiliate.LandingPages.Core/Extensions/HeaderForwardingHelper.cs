namespace Fanex.Affiliate.LandingPages;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Fanex.Affiliate.LandingPages.Core.Models;

public static class HeaderForwardingHelper
{
    private static readonly string[] ForwardHeaderNames =
    {
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
    };

    public static Dictionary<string, string> BuildForwardHeaders(HttpContext context)
    {
        var headers = new Dictionary<string, string>();

        // Fixed header - get from injected MemberSite options
        var memberSiteOptions = context.RequestServices
            .GetService(typeof(IOptions<MemberSiteOptions>)) as IOptions<MemberSiteOptions>;
        headers["ai-host"] = memberSiteOptions?.Value?.Host ?? string.Empty;

        // Dynamic origin host
        //headers["origin-host"] = $"{context.Request.Scheme}://{context.Request.Host}";
        headers["origin-host"] = "http://landing.uatasia.com/";
        // Forward specific headers if present
        foreach (var header in ForwardHeaderNames)
        {
            if (context.Request.Headers.TryGetValue(header, out var value))
            {
                headers[header] = value.ToString();
            }
        }

        return headers;
    }


}
