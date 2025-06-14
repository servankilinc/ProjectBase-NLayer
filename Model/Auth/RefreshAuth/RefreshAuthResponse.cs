using Core.Utils.Auth;
using Model.Dtos.User_;

namespace Model.Auth.RefreshAuth;

public class RefreshAuthResponse
{
    public UserBasicResponseDto User { get; set; } = null!;
    public AccessToken AccessToken { get; set; } = null!;
}

public class RefreshAuthTrustedResponse : RefreshAuthResponse
{
    public string RefreshToken { get; set; } = null!;
}