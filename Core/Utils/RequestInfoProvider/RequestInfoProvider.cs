using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Core.Utils.RequestInfoProvider;

public class RequestInfoProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public RequestInfoProvider(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;
    
    public string? GetUserId()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
    public string? GetUserAgent()
    {
        return _httpContextAccessor.HttpContext?.Request.Headers.UserAgent.ToString();
    }
    public string? GetClientIp()
    {
        return _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
    }
}
