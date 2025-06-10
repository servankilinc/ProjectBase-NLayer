using Core.Utils.Auth;

namespace Model.Auth.RefreshAuth;

public class RefreshAuthResponse
{
    public AccessToken AccessToken { get; set; } = null!;
    public RefreshTokenBase RefreshToken { get; set; } = null!;
}