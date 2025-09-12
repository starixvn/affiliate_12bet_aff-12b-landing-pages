using Fanex.Affiliate.LandingPages.Core.Commands;
using Fanex.Affiliate.LandingPages.Core.Models;
using Fanex.Affiliate.LandingPages.Core.Services.Interfaces;
using Fanex.Data.Repository;

namespace Fanex.Affiliate.LandingPages.Core.Services;

public class TrackingSignUpService(IDynamicRepository dynamicRepository,
        IRabbitBus rabbitBus,
        IRedisService redisService,
        ILogService logService) : ITrackingSignUpService
{
    public async Task WriteLogSignUp(SignUpRequest signUpRequest)
    {
        var isSuceess = await rabbitBus.PublishAsync(signUpRequest);
        if (!isSuceess)
        {
            try
            {
                await redisService.ListRightPushAsync(CacheKey.SignUp, signUpRequest);
            }
            catch (Exception ex)
            {
                await logService.SentryErrorAsync(ex);
                await WriteLogSignUpToDatabase(signUpRequest);
            }
        }
    }

    public async Task<CommandResult> WriteLogSignUpToDatabase(SignUpRequest signUpRequest)
    {
        var signUp = signUpRequest.SignUpData!;
        try
        {
            var command = new InsertSignUpLogCommand
            {
                Username = signUp.Username,
                Email = signUp.Email,
                PhoneNo = signUp.PhoneNo,
                AreaCode = signUp.AreaCode,
                CurrencyId = signUp.CurrencyId,
                CaptchaType = signUp.CaptchaType,
                ReCaptchaToken = signUp.ReCaptchaToken,
                AffClickId = signUp.AffClickId,
                AffCode = signUp.AffCode,
                UserLang = signUp.UserLang,
                ReferralUrl = signUp.ReferralUrl,
                IpAddress = signUpRequest.IpAddress,
                Duration = signUpRequest.Duration
            };

            await dynamicRepository.ExecuteAsync(command);
            return new CommandResult
            {
                ErrorCode = command.ErrCode,
                ErrorMessage = command.ErrMsg ?? string.Empty
            };
        }
        catch (Exception ex)
        {
            await logService.SentryErrorAsync(ex);
            return new CommandResult
            {
                ErrorCode = 1,
                ErrorMessage = ex.Message ?? string.Empty
            };
        }
        
        
    }
}
