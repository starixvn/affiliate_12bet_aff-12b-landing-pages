using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.PaymentMethod;

public class PaymentMethodViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        const string model = "PaymentMethod";
        return View("PaymentMethod", model);
    }
}
