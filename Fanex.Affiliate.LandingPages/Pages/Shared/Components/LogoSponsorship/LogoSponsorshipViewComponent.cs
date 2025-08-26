using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.LogoSponsorship
{
    public class LogoSponsorshipViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var model = "Logo & Sponsorship";
            return View("LogoSponsorship", model);
        }
    }
}
