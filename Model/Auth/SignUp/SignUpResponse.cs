using Core.Utils.Auth;
using Model.Dtos.User_;

namespace Model.Auth.SignUp;

public class SignUpResponse
{
    public UserBasicResponseDto User { get; set; } = null!;
    public AccessToken AccessToken { get; set; } = null!;
    public IList<string>? Roles { get; set; }
}

public class SignUpTrustedResponse : SignUpResponse
{
    public string RefreshToken { get; set; } = null!;
}
