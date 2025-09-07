using Fanex.Affiliate.LandingPages.Core.Models;

namespace Fanex.Affiliate.LandingPages.Core.Services.Interfaces;

public interface ISignUpService
{
    Task<RecaptchaInfo> GetRecaptchaInfoAsync();

    Task<NumberCaptchaResponse> GetNumberCaptchaAsync();

    Task<ExistsEmailResponse> CheckExistsEmailAsync(string email);

    Task<AffiliateCheckResponse> CheckAffiliateAsync(string affCode);

    Task<ExistsUserNameResponse> CheckExistsUserNameAsync(string username, string lang);
}
