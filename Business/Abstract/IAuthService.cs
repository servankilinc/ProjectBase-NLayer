using Model.Auth.Login;
using Model.Auth.RefreshAuth;
using Model.Auth.SignUp;

namespace Business.Abstract;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken = default);
    Task<SignUpResponse> SignUpAsync(SignUpRequest signUpRequest, CancellationToken cancellationToken = default);
    Task<RefreshAuthResponse> RefreshAuthAsync(RefreshAuthRequest refreshAuthRequest, CancellationToken cancellationToken = default);
    Task LoginWebBaseAsync(LoginRequest loginRequest, CancellationToken cancellationToken = default);
    Task SignUpWebBaseAsync(SignUpRequest signUpRequest, CancellationToken cancellationToken = default);
}