using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.RegisterForm
{
    public class LoginButtonViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var model = "LoginButton";
            return View("LoginButton", model);
        }
    }
}
