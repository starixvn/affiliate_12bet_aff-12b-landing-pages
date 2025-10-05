using Fanex.Affiliate.LandingPages.Core.Extensions;
using Fanex.Affiliate.LandingPages.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SignUpController(ISignUpService signUpService, 
    IDomainSettingService domainSettingService,
    ILogService logService) : ControllerBase
{
    [HttpPost("GetNumberCaptcha")]
    public async Task<IActionResult> GetNumberCaptcha([FromForm] string userLang)
    {
        try
        {
            var result = await signUpService.GetNumberCaptchaAsync(userLang);
            var logMessage = GetLogMessage(HttpContext);
            if (string.IsNullOrWhiteSpace(result?.Data?.Picture))
            {
                var logErrorMeassage = $"GetNumberCaptcha error: {logMessage}";
                await logService.SentryInfoAsync(logErrorMeassage);
                logService.LogFileMessage(logErrorMeassage);
            }
            else
            {
                logService.LogFileMessage($"GetNumberCaptcha success: {logMessage}");
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Internal server error", message = ex.Message });
        }
    }

    [HttpPost("ExistsEmail")]
    public async Task<IActionResult> CheckExistsEmail([FromForm] string email, [FromForm] string userLang)
    {
        try
        {
            var result = await signUpService.CheckExistsEmailAsync(email, userLang);
            if (result.Data)
            {
                return new JsonResult(false);
            }
            return new JsonResult(true);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Internal server error", message = ex.Message });
        }
    }

    [HttpPost("AffiliateCheck")]
    public async Task<IActionResult> CheckAffiliate([FromForm] string affCode, [FromForm] string userLang)
    {
        try
        {
            var result = await signUpService.CheckAffiliateAsync(affCode, userLang);
            
            if (result.ErrorCode != 0)
            {
                return new JsonResult(false);
            }
            return new JsonResult(true);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Internal server error", message = ex.Message });
        }
    }

    [HttpPost("ExistsUsername")]
    public async Task<IActionResult> CheckExistsUsername([FromForm] string username, [FromForm] string userLang)
    {
        try
        {
            var result = await signUpService.CheckExistsUserNameAsync(username, userLang);
            
            if (result.Data)
            {
                return new JsonResult(false);
            }
            return new JsonResult(true);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Internal server error", message = ex.Message });
        }
    }

    private string GetLogMessage(HttpContext httpContext)
    {
        var ipAddress = domainSettingService.GetClientIpAddress(httpContext);
        var domain = domainSettingService.GetCurrentLandingPageDomain(httpContext.Request);
        var header = HeaderForwardingHelper.BuildForwardHeaders(httpContext);
        var headersString = string.Join("; ", header.Select(h => $"{h.Key}: {h.Value}"));
        return $"domain: {domain} - ip: {ipAddress} - headers: {headersString}";
    }
}
