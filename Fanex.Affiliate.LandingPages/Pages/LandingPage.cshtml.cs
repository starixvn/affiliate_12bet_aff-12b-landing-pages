using Fanex.Affiliate.LandingPages.Core.Models;
using Fanex.Affiliate.LandingPages.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Fanex.Affiliate.LandingPages.Pages;

public class LandingPageModel(
    ILandingPageService landingPageService,
    ISignUpService signUpService,
    IConfiguration configuration,
    ILogger<LandingPageModel> logger) : PageModel
{
    #region Constants

    private const string ClickIdKey = "ClickId";
    private const string AffCodeKey = "AffCode";
    private const string RefererHeader = "Referer";
    private const string UserAgentHeader = "User-Agent";
    private const string SetCookieHeader = "Set-Cookie";
    private const int TurnstileCaptchaType = 2;
    private const int NumberCaptchaType = 0;
    private const int MinimumDomainParts = 2;

    #endregion

    #region Configuration Keys

    private static class ConfigKeys
    {
        public const string DefaultPath = "MemberSite:Redirect:DefaultPath";
        public const string RedirectToRootDomain = "MemberSite:Redirect:RedirectToRootDomain";
        public const string SpecificDomain = "MemberSite:Redirect:SpecificDomain";
    }

    #endregion

    #region Properties

    [BindProperty(SupportsGet = true)]
    public required string PageCode { get; set; }

    [BindProperty(SupportsGet = true)]
    public required string LanguageCode { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Version { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? ClickId { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? AffCode { get; set; }

    [BindProperty]
    public RegisterForm RegisterForm { get; set; } = new();

    public LandingPage? LandingPage { get; set; }

    #endregion

    #region Public Methods

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            await HandleTrackingParametersAsync();

            var (captchaInfo, landingPage) = await LoadPageDataAsync();
            LandingPage = landingPage;

            var redirectResult = await HandleRedirectsAsync(landingPage);
            if (redirectResult != null)
            {
                return redirectResult;
            }

            ConfigureFormData(captchaInfo, landingPage);

            return Page();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while loading landing page for PageCode: {PageCode}, LanguageCode: {LanguageCode}", 
                PageCode, LanguageCode);
            return RedirectToErrorPage();
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (!ModelState.IsValid || RegisterForm == null)
            {
                await OnGetAsync();
                return Page();
            }

            var result = await ProcessRegistrationAsync();
            if (result.Success)
            {
                return await HandleSuccessfulRegistrationAsync(result);
            }

            ModelState.AddModelError(string.Empty, result.ErrorMessage);
            await OnGetAsync();
            return Page();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred during form submission for PageCode: {PageCode}", PageCode);
            ModelState.AddModelError(string.Empty, "An error occurred while processing your request. Please try again.");
            await OnGetAsync();
            return Page();
        }
    }

    #endregion

    #region Private Methods
    private async Task HandleTrackingParametersAsync()
    {
        ClickId = await GetOrSetTrackingParameterAsync(ClickIdKey, ClickId);
        AffCode = await GetOrSetTrackingParameterAsync(AffCodeKey, AffCode);
    }

    private async Task<string?> GetOrSetTrackingParameterAsync(string key, string? currentValue)
    {
        if (string.IsNullOrEmpty(currentValue))
        {
            if (TempData.ContainsKey(key))
            {
                currentValue = TempData[key]?.ToString();
                TempData.Keep(key);
            }
        }
        else
        {
            TempData[key] = currentValue;
        }

        return await Task.FromResult(currentValue);
    }

    private async Task<(RecaptchaInfo captchaInfo, LandingPage? landingPage)> LoadPageDataAsync()
    {
        var captchaTask = signUpService.GetRecaptchaInfoAsync();
        var landingPageTask = landingPageService.GetLandingPage(PageCode, false);

        await Task.WhenAll(captchaTask, landingPageTask);

        return (await captchaTask, await landingPageTask);
    }

    private Task<IActionResult?> HandleRedirectsAsync(LandingPage? landingPage)
    {
        if (landingPage == null)
        {
            var redirectLink = GetDefaultMemberSiteLink();
            return Task.FromResult<IActionResult?>(Redirect(redirectLink));
        }

        if (!string.Equals(LanguageCode, landingPage.TargetLanguageCode, StringComparison.OrdinalIgnoreCase))
        {
            var correctUrl = GetCorrectLanguageLink(landingPage.TargetLanguageCode);
            return Task.FromResult<IActionResult?>(RedirectPermanent(correctUrl));
        }

        return Task.FromResult<IActionResult?>(null);
    }

    private void ConfigureFormData(RecaptchaInfo captchaInfo, LandingPage? landingPage)
    {
        if (landingPage == null)
        {
            return;
        }

        RegisterForm.CaptchaType = captchaInfo.Data?.Type == TurnstileCaptchaType ? TurnstileCaptchaType : NumberCaptchaType;
        LandingPage!.IsTurnstileCaptchaEnabled = captchaInfo.IsTurnstileCaptchaEnabled;
        LandingPage.TurnstileSiteKey = captchaInfo.Data?.SiteKey ?? string.Empty;

        RegisterForm.AreaCode = landingPage.CountryInfo?.DialCode ?? string.Empty;
        RegisterForm.CurrencyId = landingPage.CurrencyId;
        RegisterForm.AffClickId = ClickId ?? string.Empty;
        RegisterForm.AffCode = AffCode ?? string.Empty;
    }

    private async Task<RegistrationResult> ProcessRegistrationAsync()
    {
        if (RegisterForm == null)
        {
            return new RegistrationResult(false, "Registration form is not available");
        }

        RegisterForm.UserPwd = RegisterForm.Password;
        RegisterForm.UserLang = LanguageCode;
        RegisterForm.AffClickId = ClickId ?? string.Empty;
        RegisterForm.ReferralUrl = GetRequestHeader(RefererHeader);
        RegisterForm.UserAgent = GetRequestHeader(UserAgentHeader);

        var result = await signUpService.SignUpAsync(RegisterForm);
        
        if (result.ErrorCode == 0)
        {
            return new RegistrationResult(true, string.Empty, result);
        }

        return new RegistrationResult(false, result.Message);
    }

    private async Task<IActionResult> HandleSuccessfulRegistrationAsync(RegistrationResult result)
    {
        AppendSetCookie(result.Response!);
        var redirectLink = GetRedirectLink();
        return await Task.FromResult(Redirect(redirectLink));
    }

    private void AppendSetCookie(BaseResponse result)
    {
        if (result.Cookies?.Any() == true)
        {
            foreach (var cookie in result.Cookies)
            {
                Response.Headers.Append(SetCookieHeader, cookie);
            }
        }
    }

    private string GetRequestHeader(string headerName)
    {
        return HttpContext.Request.Headers.TryGetValue(headerName, out var headerValue) 
            ? headerValue.ToString() 
            : string.Empty;
    }

    private string GetDefaultMemberSiteLink()
    {
        var redirectLink = GetRedirectLink();
        var defaultPath = configuration[ConfigKeys.DefaultPath] ?? "join";
        return $"{redirectLink}/{LanguageCode}/{defaultPath}";
    }

    private string GetRedirectLink()
    {
        var redirectLink = GetRootDomain();
        var redirectToRootDomain = configuration.GetValue<bool>(ConfigKeys.RedirectToRootDomain);

        if (!redirectToRootDomain)
        {
            var specificDomain = configuration[ConfigKeys.SpecificDomain];
            if (!string.IsNullOrEmpty(specificDomain))
            {
                redirectLink = specificDomain;
            }
        }

        return $"{redirectLink}/{LanguageCode}";
    }

    private string GetRootDomain()
    {
        var request = HttpContext.Request;
        var host = request.Host.Host;
        var parts = host.Split('.', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length >= MinimumDomainParts)
        {
            var rootDomain = string.Join(".", parts[^2], parts[^1]);
            return $"{request.Scheme}://{rootDomain}";
        }

        return $"{request.Scheme}://{host}";
    }

    private string GetCorrectLanguageLink(string targetLanguageCode)
    {
        var routeValues = new Dictionary<string, object?>(RouteData.Values, StringComparer.OrdinalIgnoreCase)
        {
            ["languageCode"] = targetLanguageCode
        };

        var queryString = Request.QueryString.HasValue ? Request.QueryString.Value : string.Empty;
        var newPath = Url.Page(null, null, routeValues, Request.Scheme);

        return $"{newPath}{queryString}";
    }

    private IActionResult RedirectToErrorPage()
    {
        return Redirect("/Error");
    }

    #endregion

    #region Helper Classes
    private sealed record RegistrationResult(bool Success, string ErrorMessage, BaseResponse? Response = null);

    #endregion
}

