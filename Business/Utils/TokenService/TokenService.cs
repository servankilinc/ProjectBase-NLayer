using Core.Utils.Auth;
using Core.Utils.CrossCuttingConcerns;
using Core.Utils.RequestInfoProvider;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Model.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Business.Utils.TokenService;

[ExceptionHandler]
public class TokenService : ITokenService
{
    private readonly UserManager<User> _userManager;
    private readonly TokenSettings _tokenSettings;
    private readonly RequestInfoProvider _requestInfoProvider;
    public TokenService(UserManager<User> userManager, TokenSettings tokenSettings, RequestInfoProvider requestInfoProvider)
    {
        _userManager = userManager;
        _tokenSettings = tokenSettings;
        _requestInfoProvider = requestInfoProvider;
    }


    public async Task<AccessToken> CreateAccessToken(User user, IList<string> roles)
    {
        var claims = await GetClaimsAsync(user, roles);

        DateTime expiration = DateTime.UtcNow.AddMinutes(_tokenSettings.AccessTokenExpiration);
        SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.SecurityKey));
        SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

        JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
            issuer: _tokenSettings.Issuer,
            audience: _tokenSettings.Audience,
            claims: claims,
            expires: expiration,
            signingCredentials: signingCredentials
        );

        string? token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        return new AccessToken(token, expiration);
    }

    public RefreshToken CreateRefreshToken(User user)
    {
        return new RefreshToken
        {
            UserId = user.Id,
            IpAddress = _requestInfoProvider.GetClientIp()?.Trim() ?? string.Empty,
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            ExpirationUtc = DateTime.UtcNow.AddMinutes(_tokenSettings.RefreshTokenExpiration),
            CreateDateUtc = DateTime.UtcNow,
            TTL = _tokenSettings.RefreshTokenTTL
        };
    }


    #region Helpers
    private async Task<IList<Claim>> GetClaimsAsync(User user, IList<string> roles)
    {
        List<Claim> claimList = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, $"{user.Name} {user.LastName}")
        };

        if (!string.IsNullOrEmpty(user.Email))
            claimList.Add(new Claim(ClaimTypes.Email, user.Email));

        IList<Claim>? persistentClaims = await _userManager.GetClaimsAsync(user);
        claimList.AddRange(persistentClaims);

        IEnumerable<Claim>? roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role));
        claimList.AddRange(roleClaims);

        return claimList;
    }
    #endregion
}
