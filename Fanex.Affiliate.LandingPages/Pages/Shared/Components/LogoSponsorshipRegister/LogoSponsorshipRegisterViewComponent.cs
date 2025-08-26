using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.LogoSponsorshipRegister
{
    public class LogoSponsorshipRegisterViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var model = "Logo & Sponsorship & Register";
            return View("LogoSponsorshipRegister", model);
        }
    }
}
