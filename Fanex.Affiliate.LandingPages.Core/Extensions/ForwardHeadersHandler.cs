using Microsoft.AspNetCore.Http;

namespace Fanex.Affiliate.LandingPages.Core.Extensions;

public class ForwardHeadersHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ForwardHeadersHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var context = _httpContextAccessor.HttpContext;
        if (context != null)
        {
            var headers = HeaderForwardingHelper.BuildForwardHeaders(context);

            foreach (var header in headers)
            {
                // Tránh add trùng header
                if (!request.Headers.Contains(header.Key))
                {
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }
        }

        return base.SendAsync(request, cancellationToken);
    }
}
