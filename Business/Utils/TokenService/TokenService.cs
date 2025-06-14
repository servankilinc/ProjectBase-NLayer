using Core.Utils.Auth;
using Core.Utils.CrossCuttingConcerns;
using Core.Utils.ExceptionHandle.Exceptions;
using Core.Utils.HttpContextManager;
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
    private readonly TokenSettings _tokenSettings;
    private readonly HttpContextManager _httpContextManager;
    public TokenService(TokenSettings tokenSettings, HttpContextManager httpContextManager)
    {
        _tokenSettings = tokenSettings;
        _httpContextManager = httpContextManager;
    }


    public AccessToken GenerateAccessToken(IList<Claim> claims)
    {
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

    public RefreshToken GenerateRefreshToken(User user)
    {
        string? ipAddress = _httpContextManager.GetClientIp();
        if (string.IsNullOrEmpty(ipAddress)) throw new GeneralException("Could not read client ip address for generating refresh token!");

        return new RefreshToken
        {
            UserId = user.Id,
            IpAddress = ipAddress.Trim(),
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            ExpirationUtc = DateTime.UtcNow.AddMinutes(_tokenSettings.RefreshTokenExpiration),
            CreateDateUtc = DateTime.UtcNow,
            TTL = _tokenSettings.RefreshTokenTTL
        };
    }
}
