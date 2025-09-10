using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.HeroImage;

public class HeroImageViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        const string model = "Hero Image";
        return View("HeroImage", model);
    }
}
