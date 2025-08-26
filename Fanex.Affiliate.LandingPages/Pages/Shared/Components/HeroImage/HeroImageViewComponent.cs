using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.RegisterForm
{
    public class HeroImageViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            // Logic load data, ví dụ demo
            var model = "Hero Image";
            return View("HeroImage", model);
        }
    }
}
