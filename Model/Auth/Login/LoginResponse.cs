using Core.Utils.Auth;
using Model.Dtos.User_;

namespace Model.Auth.Login;

public class LoginResponse
{
    public UserBasicResponseDto User { get; set; } = null!;
    public AccessToken AccessToken { get; set; } = null!;
    public IList<string>? Roles { get; set; }
}

public class LoginTrustedResponse : LoginResponse
{
    public string RefreshToken { get; set; } = null!;
}