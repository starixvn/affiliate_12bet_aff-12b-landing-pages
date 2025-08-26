using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.RegisterForm
{
    public class YoutubeViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var model = "Youtube";
            return View("Youtube", model);
        }
    }
}
