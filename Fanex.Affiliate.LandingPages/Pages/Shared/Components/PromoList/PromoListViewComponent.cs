using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.PromoList;

public class PromoListViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        const string model = "Promo List";
        return View("PromoList", model);
    }
}
