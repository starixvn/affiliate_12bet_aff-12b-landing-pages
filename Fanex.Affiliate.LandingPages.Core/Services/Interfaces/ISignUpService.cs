using Fanex.Affiliate.LandingPages.Core.Models;

namespace Fanex.Affiliate.LandingPages.Core.Services.Interfaces;

public interface ISignUpService
{
    Task<RecaptchaInfo> GetRecaptchaInfoAsync(string langCode, string domain);

    Task<NumberCaptchaResponse> GetNumberCaptchaAsync(string langCode);

    Task<ExistsEmailResponse> CheckExistsEmailAsync(string email, string langCode);

    Task<AffiliateCheckResponse> CheckAffiliateAsync(string affCode, string langCode);

    Task<ExistsUserNameResponse> CheckExistsUserNameAsync(string username, string langCode);

    Task<SignUpResponse> SignUpAsync(RegisterForm registerForm);
}
