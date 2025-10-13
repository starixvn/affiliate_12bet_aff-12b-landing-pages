namespace Fanex.Affiliate.LandingPages.Controllers;

using Fanex.Affiliate.LandingPages.Core.Extensions;
using Fanex.Affiliate.LandingPages.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class WhoamiController(ILogProvider logService) : Controller
{
    [Route("healthz")]
    [AllowAnonymous]
    public IActionResult Whoami()
    {
        return Content(WhoamiHelper.GetContent(HttpContext));
    }

    [Route("sentry")]
    [AllowAnonymous]
    public async Task<IActionResult> Sentry()
    {
        await logService.SentryErrorAsync("Landing Page Test Sentry");
        return Ok("Landing Page Test sentry!");
    }

    [Route("/")]
    [AllowAnonymous]
    public IActionResult Home()
    {
        return Ok("Landing Pages...");
    }

    [Route("robots.txt")]
    [AllowAnonymous]
    public IActionResult RobotsTxt()
    {
        const string content = @"User-agent: *
Allow: /";
        return Content(content, "text/plain");
    }
}