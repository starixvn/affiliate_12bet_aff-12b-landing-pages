using Fanex.Affiliate.LandingPages.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Affiliate.LandingPages.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SignUpController(ISignUpService signUpService) : ControllerBase
{
    [HttpGet("GetNumberCaptcha")]
    public async Task<IActionResult> GetNumberCaptcha()
    {
        try
        {
            var result = await signUpService.GetNumberCaptchaAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Internal server error", message = ex.Message });
        }
    }

    [HttpGet("ExistsEmail")]
    public async Task<IActionResult> CheckExistsEmail([FromQuery(Name = "RegisterForm.Email")] string email)
    {
        try
        {
            var result = await signUpService.CheckExistsEmailAsync(email);
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
    public async Task<IActionResult> CheckAffiliate([FromQuery(Name = "RegisterForm.AffCode")] string affCode)
    {
        try
        {
            var result = await signUpService.CheckAffiliateAsync(affCode);
            
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
    public async Task<IActionResult> CheckExistsUsername([FromQuery(Name = "RegisterForm.Username")] string username)
    {
        try
        {
            var result = await signUpService.CheckExistsUserNameAsync(username, "en");

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
