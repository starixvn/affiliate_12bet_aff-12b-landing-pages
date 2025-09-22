using Fanex.Affiliate.LandingPages.Core.Services.Interfaces;

namespace Fanex.Affiliate.LandingPages.Middleware;

public class UnderMaintenanceMiddleware(RequestDelegate next,
    IDomainSettingService domainSettingService,
    IUnderMaintenanceService underMaintainService,
    ILogService logService)
{
    public async Task InvokeAsync(HttpContext context)
    {
        await next(context);

        if (context.Response.StatusCode == 404)
        {
            await HandleNotFoundAsync(context);
        }

        await HandleCheckMaintenanceAsync(context);
    }

    private async Task HandleNotFoundAsync(HttpContext context)
    {
        var requestPath = context.Request.Path.Value ?? string.Empty;
        if (ShouldIgnorePath(requestPath))
        {
            return;
        }

        await HandleRedirectAsync(context, requestPath, "404 Not Found");
    }

    private async Task HandleCheckMaintenanceAsync(HttpContext context)
    {
        var requestPath = context.Request.Path.Value ?? string.Empty;
        if (ShouldIgnorePath(requestPath))
        {
            return;
        }

        var currentDomain = domainSettingService.GetCurrentLandingPageDomain(context.Request);
        var isUnderMaintenance = await underMaintainService.GetIsUnderMaintenanceByDomainAsync(currentDomain);
        if (isUnderMaintenance)
        {
            await HandleRedirectAsync(context, requestPath, "Under Maintenance");
        }
    }
    private async Task HandleRedirectAsync(HttpContext context, string requestPath, string errorTitle)
    {
        var requestMethod = context.Request.Method;
        var userAgent = context.Request.Headers.UserAgent.ToString();
        var referer = context.Request.Headers.Referer.ToString();
        var remoteIp = GetClientIpAddress(context);
        var currentDomain = domainSettingService.GetCurrentLandingPageDomain(context.Request);

        await logService.SentryInfoAsync(
            $"{errorTitle}: {requestMethod} {currentDomain} {requestPath} from IP {remoteIp} with User-Agent: " +
                $"{userAgent} and Referer: {referer}");

        var redirectLink = domainSettingService.GetCurrentMemberSiteHost(context.Request);
        context.Response.Redirect(redirectLink);
    }

    private static bool ShouldIgnorePath(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return true;
        }

        // Ignore static file extensions
        var staticFileExtensions = new[]
        {
            ".css", ".js", ".png", ".jpg", ".jpeg", ".gif", ".ico", ".svg", ".woff", ".woff2", ".ttf", ".eot"
        };

        if (Array.Exists(staticFileExtensions, ext => path.EndsWith(ext, StringComparison.OrdinalIgnoreCase)))
        {
            return true;
        }

        // Ignore API endpoints
        if (path.StartsWith("/api/", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        // Ignore health check endpoints
        if (path.Equals("/healthz", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        // Ignore favicon requests
        if (path.Equals("/favicon.ico", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return false;
    }

    private static string GetClientIpAddress(HttpContext context)
    {
        var realIp = context.Request.Headers["x-actual-ip"].FirstOrDefault();
        if (!string.IsNullOrEmpty(realIp))
        {
            return realIp;
        }
        else
        {
            var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwardedFor))
            {
                return forwardedFor.Split(',')[0].Trim();
            }
        }

        return context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
    }
}
