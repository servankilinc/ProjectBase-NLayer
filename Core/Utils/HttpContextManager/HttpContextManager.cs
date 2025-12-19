using Core.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System.Security.Claims;

namespace Core.Utils.HttpContextManager;

public class HttpContextManager
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public HttpContextManager(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

    public string? GetUserId()
    {
        if (_httpContextAccessor.HttpContext == null) throw new Exception("Not exist HttpContext inside HttpContextManager.GetUserId!");

        return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
    public string? GetUserAgent()
    {
        if (_httpContextAccessor.HttpContext == null) throw new Exception("Not exist HttpContext inside HttpContextManager.GetUserAgent!");

        return _httpContextAccessor.HttpContext.Request.Headers.UserAgent.ToString();
    }
    public string? GetClientIp()
    {
        if (_httpContextAccessor.HttpContext == null) throw new Exception("Not exist HttpContext inside HttpContextManager.GetClientIp!");

        if (_httpContextAccessor.HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
            return _httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"];
        return _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();
    }
    public bool IsMobile()
    {
        if (_httpContextAccessor.HttpContext == null) throw new Exception("Not exist HttpContext inside HttpContextManager.IsMobile!");

        var appPlatform = _httpContextAccessor.HttpContext.Request.Headers["X-App-Platform"];
        return appPlatform.ToString().ToLowerInvariant() == "mobile".ToLowerInvariant();
    }
    public string GetCurrentCulture()
    {
        string defaultCulture = "tr-TR";
        if (_httpContextAccessor.HttpContext == null) return defaultCulture;

        var cookieName = CookieRequestCultureProvider.DefaultCookieName;
        var cookieValue = _httpContextAccessor.HttpContext.Request.Cookies[cookieName];
        if (string.IsNullOrWhiteSpace(cookieValue)) return defaultCulture;

        var requestCulture = CookieRequestCultureProvider.ParseCookieValue(cookieValue);
        var cultureInfo = requestCulture?.Cultures.FirstOrDefault().Value;
        if (string.IsNullOrWhiteSpace(cultureInfo)) return defaultCulture;

        return cultureInfo;
    }
    public Languages GetCurrentLanguage()
    {
        string cultureInfo = GetCurrentCulture();
        return EnumExtensions.GetEnumByDescription<Languages>(cultureInfo);
    }
    public int GetCurrentLanguageId()
    {
        string cultureInfo = GetCurrentCulture();
        return (byte)EnumExtensions.GetEnumByDescription<Languages>(cultureInfo);
    }
    public bool SetCurrentCulture(string culture)
    {
        if (_httpContextAccessor.HttpContext == null) throw new Exception("Not exist HttpContext inside HttpContextManager.SetCurrentCulture!");
        var cookieName = CookieRequestCultureProvider.DefaultCookieName;
        var cookieValue = CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture));
        _httpContextAccessor.HttpContext.Response.Cookies.Append(cookieName, cookieValue, new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddYears(1),
            IsEssential = true,
            Secure = true,
            HttpOnly = false,
            SameSite = SameSiteMode.Lax,
        });
        return true;
    }
    public void AddRefreshTokenToCookie(string refreshToken, DateTime expirationUtc)
    {
        if (_httpContextAccessor.HttpContext == null) throw new Exception("Not exist HttpContext inside HttpContextManager.AddRefreshTokenToCookie!");

        _httpContextAccessor.HttpContext.Response.Cookies.Append("RefreshToken", refreshToken, new CookieOptions
        {
            Secure = true,
            HttpOnly = true,
            Expires = expirationUtc,
            SameSite = SameSiteMode.Lax,
            //Path = "/Account/RefreshAuth"
        });
    }
    public string GetRefreshTokenFromCookie()
    {
        if (_httpContextAccessor.HttpContext == null) throw new Exception("Not exist HttpContext inside HttpContextManager.GetRefreshTokenFromCookie!");

        string? refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["RefreshToken"];
        if (string.IsNullOrWhiteSpace(refreshToken)) throw new Exception("Not exist refresh token inside cookie!");

        return refreshToken;
    }
    public void DeletetRefreshTokenFromCookie()
    {
        if (_httpContextAccessor.HttpContext == null) throw new Exception("Not exist HttpContext inside HttpContextManager.DeletetRefreshTokenFromCookie!");

        _httpContextAccessor.HttpContext?.Response.Cookies.Delete("Key_RefreshToken");
    }
}