using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.PromoList;

public class PromoListViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(LandingPageModel model)
    {
        return View("PromoList", model);
    }
}
