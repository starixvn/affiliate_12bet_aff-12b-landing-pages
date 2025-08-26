using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.RegisterForm
{
    public class PaymentMethodViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var model = "PaymentMethod";
            return View("PaymentMethod", model);
        }
    }
}
