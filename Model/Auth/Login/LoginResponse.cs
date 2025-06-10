using Core.Utils.Auth;
using Model.Entities;

namespace Model.Auth.Login;

public class LoginResponse
{
    public User User { get; set; } = null!;
    public AccessToken AccessToken { get; set; } = null!;
    public RefreshTokenBase RefreshToken { get; set; } = null!;
    public IList<string>? Roles { get; set; }
}
