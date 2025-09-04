using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.RegisterForm
{
    public class CTAButtonViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            // Logic load data, ví dụ demo
            var model = "CTA Button";
            return View("CTAButton", model);
        }
    }
}
