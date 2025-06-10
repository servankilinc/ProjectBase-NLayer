using Core.Utils.Auth;
using Model.Entities;

namespace Business.Utils.TokenService;

public interface ITokenService
{
    Task<AccessToken> CreateAccessToken(User user, IList<string> roles);
    RefreshToken CreateRefreshToken(User user);
}