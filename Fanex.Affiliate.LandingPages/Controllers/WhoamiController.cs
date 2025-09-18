namespace Fanex.Affiliate.LandingPages.Controllers;

using Fanex.Affiliate.LandingPages.Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class WhoamiController : Controller
{
    [Route("healthz")]
    [AllowAnonymous]
    public IActionResult Whoami()
    {
        return Content(WhoamiHelper.GetContent(HttpContext));
    }
}