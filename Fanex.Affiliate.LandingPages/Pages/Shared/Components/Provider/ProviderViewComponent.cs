using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.Provider;

public class ProviderViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(LandingPageModel model)
    {
        return View("Provider", model);
    }
}
