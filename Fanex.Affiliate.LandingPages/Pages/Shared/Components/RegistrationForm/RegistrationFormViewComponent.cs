using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.RegisterForm
{
    public class RegistrationFormViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(LandingPageModel model)
        {
            // Logic load data, ví dụ demo
            return View("RegistrationForm", model);
        }
    }
}
