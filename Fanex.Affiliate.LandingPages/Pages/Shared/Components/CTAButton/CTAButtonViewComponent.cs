using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.CTAButton;

public class CTAButtonViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        const string model = "CTA Button";
        return View("CTAButton", model);
    }
}
