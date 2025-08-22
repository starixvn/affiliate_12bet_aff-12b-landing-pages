using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.RegisterForm
{
    public class BackgroundViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            // Logic load data, ví dụ demo
            var model = "Background";
            return View("Background", model);
        }
    }
}
