using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.PaymentMethod;

public class PaymentMethodViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(LandingPageModel model)
    {
        return View("PaymentMethod", model);
    }
}
