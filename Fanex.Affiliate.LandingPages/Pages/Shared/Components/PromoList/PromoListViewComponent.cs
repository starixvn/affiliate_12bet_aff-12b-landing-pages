using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.RegisterForm
{
    public class PromoListViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var model = "Promo List";
            return View("PromoList", model);
        }
    }
}
