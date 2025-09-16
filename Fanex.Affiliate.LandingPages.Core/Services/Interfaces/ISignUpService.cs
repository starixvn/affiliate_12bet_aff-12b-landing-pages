using Fanex.Affiliate.LandingPages.Core.Models;

namespace Fanex.Affiliate.LandingPages.Core.Services.Interfaces;

public interface ISignUpService
{
    Task<RecaptchaInfo> GetRecaptchaInfoAsync(string lang);

    Task<NumberCaptchaResponse> GetNumberCaptchaAsync(string lang);

    Task<ExistsEmailResponse> CheckExistsEmailAsync(string email, string lang);

    Task<AffiliateCheckResponse> CheckAffiliateAsync(string affCode, string lang);

    Task<ExistsUserNameResponse> CheckExistsUserNameAsync(string username, string lang);

    Task<SignUpResponse> SignUpAsync(RegisterForm registerForm);
}
