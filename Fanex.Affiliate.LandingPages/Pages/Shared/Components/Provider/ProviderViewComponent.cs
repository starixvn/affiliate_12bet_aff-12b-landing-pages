using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.RegisterForm
{
    public class ProviderViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var model = "Provider";
            return View("Provider", model);
        }
    }
}
