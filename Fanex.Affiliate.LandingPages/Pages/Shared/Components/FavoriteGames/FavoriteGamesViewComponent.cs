using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Pages.Shared.Components.FavoriteGames;

public class FavoriteGamesViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        const string model = "Favorite Games";
        return View("FavoriteGames", model);
    }
}
