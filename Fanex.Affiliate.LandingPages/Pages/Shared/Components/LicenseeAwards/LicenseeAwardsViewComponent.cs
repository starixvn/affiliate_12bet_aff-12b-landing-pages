using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.LicenseeAwards;

public class LicenseeAwardsViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        const string model = "Licensee Awards";
        return View("LicenseeAwards", model);
    }
}
