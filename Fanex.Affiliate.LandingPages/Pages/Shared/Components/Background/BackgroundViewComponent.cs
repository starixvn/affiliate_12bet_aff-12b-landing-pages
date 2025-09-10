using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.Background;

public class BackgroundViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        const string model = "Background";
        return View("Background", model);
    }
}
