namespace Fanex.Affiliate.LandingPages.Core.Services.Interfaces;

public interface ISignUpService
{
    Task SentryErrorAsync(string errMsg, Exception exception);
}
