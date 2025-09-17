using Fanex.Affiliate.LandingPages.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SignUpController(ISignUpService signUpService) : ControllerBase
{
    [HttpGet("GetNumberCaptcha")]
    public async Task<IActionResult> GetNumberCaptcha(string langCode)
    {
        try
        {
            var result = await signUpService.GetNumberCaptchaAsync(langCode);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Internal server error", message = ex.Message });
        }
    }

    [HttpGet("ExistsEmail")]
    public async Task<IActionResult> CheckExistsEmail([FromQuery(Name = "RegisterForm.Email")] string email,
        [FromQuery(Name = "RegisterForm.UserLang")] string langCode)
    {
        try
        {
            var result = await signUpService.CheckExistsEmailAsync(email, langCode);
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

    [HttpGet("AffiliateCheck")]
    public async Task<IActionResult> CheckAffiliate([FromQuery(Name = "RegisterForm.AffCode")] string affCode,
        [FromQuery(Name = "RegisterForm.UserLang")] string langCode)
    {
        try
        {
            var result = await signUpService.CheckAffiliateAsync(affCode, langCode);
            
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

    [HttpGet("ExistsUsername")]
    public async Task<IActionResult> CheckExistsUsername([FromQuery(Name = "RegisterForm.Username")] string username,
        [FromQuery(Name = "RegisterForm.UserLang")] string langCode)
    {
        try
        {
            var result = await signUpService.CheckExistsUserNameAsync(username, langCode);
            
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
}
