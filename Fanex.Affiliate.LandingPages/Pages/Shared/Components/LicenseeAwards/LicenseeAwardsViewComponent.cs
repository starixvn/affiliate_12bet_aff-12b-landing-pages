using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.RegisterForm
{
    public class LicenseeAwardsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            // Logic load data, ví dụ demo
            var model = "Licensee Awards";
            return View("LicenseeAwards", model);
        }
    }
}
