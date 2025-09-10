using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.Provider;

public class ProviderViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        const string model = "Provider";
        return View("Provider", model);
    }
}
