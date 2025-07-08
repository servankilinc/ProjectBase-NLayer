using Core.Utils.Auth;
using Model.Dtos.User_;

namespace Model.Auth.Login;

public class LoginResponse
{
    public IList<string>? Roles { get; set; }
    public AccessToken AccessToken { get; set; } = null!;
    public UserBasicResponseDto User { get; set; } = null!;
}

public class LoginTrustedResponse : LoginResponse
{
    public string RefreshToken { get; set; } = null!;
}