using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.LoginButton;

public class LoginButtonViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        const string model = "LoginButton";
        return View("LoginButton", model);
    }
}
