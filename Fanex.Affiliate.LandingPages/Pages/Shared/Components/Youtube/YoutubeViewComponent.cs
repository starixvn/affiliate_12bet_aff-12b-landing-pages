using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.Youtube;

public class YoutubeViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        const string model = "Youtube";
        return View("Youtube", model);
    }
}
