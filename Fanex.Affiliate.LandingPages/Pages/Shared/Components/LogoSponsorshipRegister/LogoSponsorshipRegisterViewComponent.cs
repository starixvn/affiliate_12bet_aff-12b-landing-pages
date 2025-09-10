using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.LogoSponsorshipRegister;

public class LogoSponsorshipRegisterViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        const string model = "Logo & Sponsorship & Register";
        return View("LogoSponsorshipRegister", model);
    }
}
