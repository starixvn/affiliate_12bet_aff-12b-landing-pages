using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.FavoriteGames;

public class FavoriteGamesViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(LandingPageModel model)
    {
        return View("FavoriteGames", model);
    }
}
