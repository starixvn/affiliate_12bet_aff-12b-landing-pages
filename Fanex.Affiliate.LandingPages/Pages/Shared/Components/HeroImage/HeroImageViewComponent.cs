using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.HeroImage;

public class HeroImageViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(LandingPageModel model)
    {
        return View("HeroImage", model);
    }
}
