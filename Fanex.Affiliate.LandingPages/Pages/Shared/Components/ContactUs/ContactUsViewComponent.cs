using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.RegisterForm
{
    public class ContactUsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            // Logic load data, ví dụ demo
            var model = "Contact Us";
            return View("ContactUs", model);
        }
    }
}
