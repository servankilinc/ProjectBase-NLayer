using Core.Utils.Auth;
using Model.Entities;
using System.Security.Claims;

namespace Business.Utils.TokenService;

public interface ITokenService
{
    AccessToken GenerateAccessToken(IList<Claim> claims);
    RefreshToken GenerateRefreshToken(User user);
}