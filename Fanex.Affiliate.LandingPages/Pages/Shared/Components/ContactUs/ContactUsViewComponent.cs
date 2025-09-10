using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.ContactUs;

public class ContactUsViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        const string model = "Contact Us";
        return View("ContactUs", model);
    }
}
