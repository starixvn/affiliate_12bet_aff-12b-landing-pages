using Fanex.Affiliate.LandingPages.Core;
using Fanex.Affiliate.LandingPages.Core.LandingPageConstants;
using Fanex.Affiliate.LandingPages.Core.Models;
using Fanex.Affiliate.LandingPages.Core.Services;
using Fanex.Affiliate.LandingPages.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PhoneNumbers;

namespace Fanex.Affiliate.LandingPages.Pages;

/// <summary>
/// Landing page model for handling user registration and page display
/// </summary>
public class LandingPageModel(
    ILandingPageService landingPageService,
    ISignUpService signUpService,
    IConfiguration configuration,
    ILogger<LandingPageModel> logger) : PageModel
{
    #region Configuration Keys

    private static class ConfigKeys
    {
        public const string DefaultPath = "MemberSite:Redirect:DefaultPath";
        public const string RedirectToRootDomain = "MemberSite:Redirect:RedirectToRootDomain";
        public const string SpecificDomain = "MemberSite:Redirect:SpecificDomain";
        public const string MediaDomain = "MediaDomain";
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

    [BindProperty(SupportsGet = true)]
    public string? FormCode { get; set; } = LandingPageConstants.DefaultFormCode;

    [BindProperty(SupportsGet = true)]
    public bool? IsPreView { get; set; } = false;

    [BindProperty(SupportsGet = true)]
    public int? PageId { get; set; }

    [BindProperty]
    public RegisterForm RegisterForm { get; set; } = new();

    public LandingPage? LandingPage { get; set; }

    #endregion

    #region Public Methods

    /// <summary>
    /// Handles GET requests for the landing page
    /// </summary>
    /// <returns>Page result or redirect</returns>
    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            ValidateRequiredParameters();
            SetCulture();
            await HandleTrackingParametersAsync();
            return await ProcessFormDataAsync();
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid parameters for landing page: PageCode={PageCode}, LanguageCode={LanguageCode}", 
                PageCode, LanguageCode);
            return RedirectToErrorPage();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{ErrorMessage} for PageCode: {PageCode}, LanguageCode: {LanguageCode}", 
                LandingPageConstants.LandingPageLoadError, PageCode, LanguageCode);
            return RedirectToErrorPage();
        }
    }

    /// <summary>
    /// Handles POST requests for form submission
    /// </summary>
    /// <returns>Page result or redirect</returns>
    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            var errorMessage = string.Empty;
            
            if (ModelState.IsValid && RegisterForm != null)
            {
                var result = await ProcessRegistrationAsync();
                if (result.Success)
                {
                    return await HandleSuccessfulRegistrationAsync(result);
                }
                else
                {
                    errorMessage = result.ErrorMessage;
                }
            }
            else
            {
                errorMessage = GetModelStateErrors();
            }

            if (errorMessage is not null)
            {
                ModelState.AddModelError(string.Empty, errorMessage);
            }

            return await ProcessFormDataAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{ErrorMessage} for PageCode: {PageCode}", 
                LandingPageConstants.FormSubmissionError, PageCode);
            ModelState.AddModelError(string.Empty, LandingPageConstants.ProcessingError);
            return RedirectToErrorPage();
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Validates required parameters
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when required parameters are missing</exception>
    private void ValidateRequiredParameters()
    {
        if (!ValidationService.ValidateRequiredString(PageCode, nameof(PageCode)))
        {
            throw new ArgumentException(ValidationService.GetRequiredParameterErrorMessage(nameof(PageCode)));
        }

        if (!ValidationService.ValidateRequiredString(LanguageCode, nameof(LanguageCode)))
        {
            throw new ArgumentException(ValidationService.GetRequiredParameterErrorMessage(nameof(LanguageCode)));
        }
    }

    /// <summary>
    /// Sets culture based on language code
    /// </summary>
    private void SetCulture()
    {
        LandingPageCultureService.SetCulture(LanguageCode);
    }

    /// <summary>
    /// Configures form data by loading landing page information
    /// </summary>
    private async Task<IActionResult> ProcessFormDataAsync()
    {
        try
        {
            LandingPage = await LoadPageDataAsync(PageCode, IsPreView, PageId);

            var redirectResult = await HandleRedirectsAsync(LandingPage);
            if (redirectResult != null)
            {
                return redirectResult;
            }
            else
            {
                ConfigureRegisterForm(LandingPage!);
                return Page();
            }
        }
        catch (Exception ex)
        {
            return Page();
        }

    }

    /// <summary>
    /// Configures the register form with landing page data
    /// </summary>
    /// <param name="landingPage">Landing page data</param>
    private void ConfigureRegisterForm(LandingPage landingPage)
    {
        RegisterForm.CaptchaType = landingPage.CaptchaType;
        RegisterForm.AreaCode = $"+{landingPage.DialCode}";
        RegisterForm.CountryCode = landingPage.TargetCountryCode;
        RegisterForm.CurrencyId = landingPage.CurrencyId;
        RegisterForm.AffClickId = ClickId ?? string.Empty;
        RegisterForm.AffCode = AffCode ?? string.Empty;
        
        var form = landingPage.Modules!.FirstOrDefault(t => t.ModuleId == (int)UiModule.RegistrationForm);
        var formCode = form?.MediaCode ?? FormCode;
        if (Request.Query.ContainsKey("FormCode"))
        {
            formCode = Request.Query["FormCode"].ToString();
        }

        RegisterForm.SignUpFormConfig = SignUpFormConfigs.SupportSignUpForms
            .FirstOrDefault(t => t.FormCode == formCode);
    }

    /// <summary>
    /// Handles tracking parameters (ClickId, AffCode)
    /// </summary>
    private async Task HandleTrackingParametersAsync()
    {
        ClickId = await GetOrSetTrackingParameterAsync(LandingPageConstants.ClickIdKey, ClickId);
        AffCode = await GetOrSetTrackingParameterAsync(LandingPageConstants.AffCodeKey, AffCode);
    }

    /// <summary>
    /// Gets or sets a tracking parameter from TempData
    /// </summary>
    /// <param name="key">Parameter key</param>
    /// <param name="currentValue">Current parameter value</param>
    /// <returns>Parameter value</returns>
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

    /// <summary>
    /// Loads landing page data from service
    /// </summary>
    /// <param name="pageCode">Page code</param>
    /// <returns>Landing page data or null if not found</returns>
    private async Task<LandingPage?> LoadPageDataAsync(string pageCode, bool? isPreview, int? pageId)
    {
        var landingPage = await landingPageService.GetLandingPage(pageCode, isPreview, pageId);
        if (landingPage == null)
        {
            return null;
        }

        await ConfigureCaptchaInfoAsync(landingPage);
        ConfigurePhoneInfo(landingPage);

        return landingPage;
    }

    /// <summary>
    /// Configures captcha information for the landing page
    /// </summary>
    /// <param name="landingPage">Landing page to configure</param>
    private async Task ConfigureCaptchaInfoAsync(LandingPage landingPage)
    {
        var captchaInfo = await signUpService.GetRecaptchaInfoAsync();
        landingPage.CaptchaType = captchaInfo.IsTurnstileCaptchaEnabled 
            ? LandingPageConstants.TurnstileCaptchaType 
            : LandingPageConstants.NumberCaptchaType;
        landingPage.IsTurnstileCaptchaEnabled = captchaInfo.IsTurnstileCaptchaEnabled;
        landingPage.TurnstileSiteKey = captchaInfo.Data?.SiteKey ?? string.Empty;
    }

    /// <summary>
    /// Configures phone information for the landing page
    /// </summary>
    /// <param name="landingPage">Landing page to configure</param>
    private static void ConfigurePhoneInfo(LandingPage landingPage)
    {
        var phoneUtil = PhoneNumberUtil.GetInstance();
        landingPage.DialCode = phoneUtil.GetCountryCodeForRegion(landingPage.TargetCountryCode);
    }

    /// <summary>
    /// Handles redirects based on landing page data
    /// </summary>
    /// <param name="landingPage">Landing page data</param>
    /// <returns>Redirect result or null if no redirect needed</returns>
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

    /// <summary>
    /// Processes user registration
    /// </summary>
    /// <returns>Registration result</returns>
    private async Task<RegistrationResult> ProcessRegistrationAsync()
    {
        if (RegisterForm == null)
        {
            return new RegistrationResult(false, LandingPageConstants.RegistrationFormNotAvailable);
        }

        ConfigureRegistrationForm();
        var result = await signUpService.SignUpAsync(RegisterForm);
        
        if (result.ErrorCode == 0)
        {
            return new RegistrationResult(true, string.Empty, result);
        }

        var errorMessage = string.IsNullOrEmpty(result.Message) ?
            $"{result.ErrorCode} - {LandingPageConstants.ProcessingError}"
            : result.Message;
        return new RegistrationResult(false, errorMessage);
    }

    /// <summary>
    /// Configures the registration form with request data
    /// </summary>
    private void ConfigureRegistrationForm()
    {
        RegisterForm.UserPwd = RegisterForm.Password;
        RegisterForm.UserLang = LanguageCode;
        RegisterForm.AffClickId = ClickId ?? string.Empty;
        RegisterForm.ReferralUrl = GetRequestHeader(LandingPageConstants.RefererHeader);
        RegisterForm.UserAgent = GetRequestHeader(LandingPageConstants.UserAgentHeader);
        RegisterForm.SignUpUrl = GetCurrentUrl().ToString();
        RegisterForm.IpAddress = GetClientIpAddress();
    }

    /// <summary>
    /// Handles successful registration by setting cookies and redirecting
    /// </summary>
    /// <param name="result">Registration result</param>
    /// <returns>Redirect result</returns>
    private async Task<IActionResult> HandleSuccessfulRegistrationAsync(RegistrationResult result)
    {
        AppendSetCookie(result.Response!);
        var redirectLink = GetMemberSiteRedirectLink();
        return await Task.FromResult(Redirect(redirectLink));
    }

    /// <summary>
    /// Appends cookies from sign-up response to HTTP response
    /// </summary>
    /// <param name="result">Sign-up response containing cookies</param>
    private void AppendSetCookie(SignUpResponse result)
    {
        if (result.Cookies?.Any() == true)
        {
            foreach (var cookie in result.Cookies)
            {
                Response.Headers.Append(LandingPageConstants.SetCookieHeader, cookie);
            }
        }
    }

    /// <summary>
    /// Gets a request header value
    /// </summary>
    /// <param name="headerName">Header name</param>
    /// <returns>Header value or empty string</returns>
    private string GetRequestHeader(string headerName)
    {
        return HttpContext.Request.Headers.TryGetValue(headerName, out var headerValue) 
            ? headerValue.ToString() 
            : string.Empty;
    }

    /// <summary>
    /// Gets the default member site link
    /// </summary>
    /// <returns>Default member site URL</returns>
    private string GetDefaultMemberSiteLink()
    {
        var redirectLink = GetMemberSiteRedirectLink();
        var defaultPath = configuration[ConfigKeys.DefaultPath] ?? LandingPageConstants.DefaultPath;
        return $"{redirectLink}/{LanguageCode}/{defaultPath}";
    }

    /// <summary>
    /// Gets the member site redirect link
    /// </summary>
    /// <returns>Member site redirect URL</returns>
    public string GetMemberSiteRedirectLink()
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

    /// <summary>
    /// Gets the root domain from the current request
    /// </summary>
    /// <returns>Root domain URL</returns>
    private string GetRootDomain()
    {
        var request = HttpContext.Request;
        var host = request.Host.Host;
        var parts = host.Split('.', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length >= LandingPageConstants.MinimumDomainParts)
        {
            var rootDomain = string.Join(".", parts[^2], parts[^1]);
            return $"{request.Scheme}://{rootDomain}";
        }

        return $"{request.Scheme}://{host}";
    }

    /// <summary>
    /// Gets the correct language link for redirects
    /// </summary>
    /// <param name="targetLanguageCode">Target language code</param>
    /// <returns>Correct language URL</returns>
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

    public string GetMediaRootDomain()
    {
        var mediaDomain = configuration[ConfigKeys.MediaDomain] ?? string.Empty;
        return mediaDomain;
    }

    /// <summary>
    /// Redirects to error page
    /// </summary>
    /// <returns>Redirect result</returns>
    private IActionResult RedirectToErrorPage()
    {
        return Redirect("/Error");
    }

    /// <summary>
    /// Gets the current request URL
    /// </summary>
    /// <returns>Current URL</returns>
    private Uri GetCurrentUrl()
    {
        var request = HttpContext.Request;
        var uriBuilder = new UriBuilder
        {
            Scheme = request.Scheme,
            Host = request.Host.Host,
            Port = request.Host.Port ?? GetDefaultPort(request.Scheme),
            Path = request.Path,
            Query = request.QueryString.HasValue ? request.QueryString.Value : string.Empty
        };
        return uriBuilder.Uri;
    }

    /// <summary>
    /// Gets the default port for a scheme
    /// </summary>
    /// <param name="scheme">URL scheme</param>
    /// <returns>Default port number</returns>
    private static int GetDefaultPort(string scheme)
    {
        return string.Equals(scheme, LandingPageConstants.HttpsScheme, StringComparison.OrdinalIgnoreCase) 
            ? LandingPageConstants.HttpsPort 
            : LandingPageConstants.HttpPort;
    }

    /// <summary>
    /// Gets the client IP address from request headers or connection
    /// </summary>
    /// <returns>Client IP address</returns>
    private string GetClientIpAddress()
    {
        var headers = HttpContext.Request.Headers;
        if (headers.TryGetValue(LandingPageConstants.XForwardedForHeader, out var forwardedFor) && 
            !string.IsNullOrWhiteSpace(forwardedFor))
        {
            var ip = forwardedFor.ToString().Split(',')[0].Trim();
            if (!string.IsNullOrEmpty(ip))
            {
                return ip;
            }
        }

        return HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
    }

    /// <summary>
    /// Gets model state validation errors as a single string
    /// </summary>
    /// <returns>Combined error message</returns>
    private string GetModelStateErrors()
    {
        var errors = ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .Where(e => !string.IsNullOrEmpty(e));

        return string.Join("; ", errors);
    }

    #endregion

    #region Helper Classes

    /// <summary>
    /// Result of registration processing
    /// </summary>
    /// <param name="Success">Whether registration was successful</param>
    /// <param name="ErrorMessage">Error message if registration failed</param>
    /// <param name="Response">Sign-up response if successful</param>
    private sealed record RegistrationResult(bool Success, string ErrorMessage, SignUpResponse? Response = null);

    #endregion
}