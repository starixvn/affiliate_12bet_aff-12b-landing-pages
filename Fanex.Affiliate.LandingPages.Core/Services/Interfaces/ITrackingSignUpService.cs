using Fanex.Affiliate.LandingPages.Core.Commands;
using Fanex.Affiliate.LandingPages.Core.Models;

namespace Fanex.Affiliate.LandingPages.Core.Services.Interfaces;

public interface ITrackingSignUpService
{
    Task WriteLogSignUp(SignUpRequest signUpRequest);

    Task<CommandResult> WriteLogSignUpToDatabase(SignUpRequest signUpRequest);
}
