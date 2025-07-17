using AutoMapper;
using Business.Abstract;
using Business.Utils.TokenService;
using Core.Utils.Auth;
using Core.Utils.CrossCuttingConcerns;
using Core.Utils.ExceptionHandle.Exceptions;
using Core.Utils.HttpContextManager;
using DataAccess.UoW;
using Microsoft.AspNetCore.Identity;
using Model.Auth.Login;
using Model.Auth.RefreshAuth;
using Model.Auth.SignUp;
using Model.Dtos.User_;
using Model.Entities;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Business.Concrete;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly HttpContextManager _httpContextManager;
    private readonly IMapper _mapper;
    public AuthService(
        IUnitOfWork unitOfWork,
        ITokenService tokenService,
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        HttpContextManager httpContextManager,
        IMapper mapper
    )
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _userManager = userManager;
        _signInManager = signInManager;
        _httpContextManager = httpContextManager;
        _mapper = mapper;
    }


    [Validation(typeof(LoginRequest))]
    public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken = default)
    {
        User? user = await _userManager.FindByEmailAsync(loginRequest.Email);
        if (user == null) throw new BusinessException("The email address is not exist.", description: $"Requester email address: {loginRequest.Email}");

        bool isPasswordValid = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
        if (!isPasswordValid) throw new BusinessException("Password does not correct.", description: $"Requester email address: {loginRequest.Email}");

        IList<string> roles = await _userManager.GetRolesAsync(user);
        IList<Claim> claims = await GetClaimsAsync(user, roles);
        AccessToken accessToken = _tokenService.GenerateAccessToken(claims);
        RefreshToken refreshToken = _tokenService.GenerateRefreshToken(user);

        string? ipAddress = _httpContextManager.GetClientIp();
        if (string.IsNullOrEmpty(ipAddress)) throw new GeneralException("Ip address could not found for login.", description: $"Requester email address: {loginRequest.Email}");

        await _unitOfWork.RefreshTokens.DeleteAndSaveAsync(where: f => f.UserId == user.Id && f.IpAddress.Trim() == ipAddress.Trim(), cancellationToken);
        await _unitOfWork.RefreshTokens.AddAndSaveAsync(refreshToken, cancellationToken);

        if (_httpContextManager.IsMobile())
        {
            return new LoginTrustedResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                User = _mapper.Map<UserBasicResponseDto>(user),
                Roles = roles
            };
        }
        else
        {
            _httpContextManager.AddRefreshTokenToCookie(refreshToken.Token, refreshToken.ExpirationUtc);
            return new LoginResponse
            {
                AccessToken = accessToken,
                User = _mapper.Map<UserBasicResponseDto>(user),
                Roles = roles
            };
        }
    }

    [Validation(typeof(SignUpRequest))]
    public async Task<SignUpResponse> SignUpAsync(SignUpRequest signUpRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            var userExist = await _userManager.FindByEmailAsync(signUpRequest.Email);
            if (userExist != null) throw new BusinessException("The email address is already in use.", description: $"Requester email address: {signUpRequest.Email}");

            var user = _mapper.Map<User>(signUpRequest);
            user.UserName = $"{signUpRequest.Email}_{DateTime.UtcNow:yyyyMMddHHmmss}";

            var result = await _userManager.CreateAsync(user, signUpRequest.Password);
            if (!result.Succeeded) throw new GeneralException(string.Join("\n", result.Errors.Select(e => e.Description)), description: $"User cannot be created. Requester email: {signUpRequest.Email}");

            var roleResult = await _userManager.AddToRoleAsync(user, "User");
            if (!roleResult.Succeeded) throw new GeneralException("Failed to assign role.", description: $"Requester email address: {signUpRequest.Email}");

            IList<string> roles = await _userManager.GetRolesAsync(user);
            IList<Claim> claims = await GetClaimsAsync(user, roles);
            AccessToken accessToken = _tokenService.GenerateAccessToken(claims);
            RefreshToken refreshToken = _tokenService.GenerateRefreshToken(user);

            await _unitOfWork.RefreshTokens.AddAndSaveAsync(refreshToken, cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            if (_httpContextManager.IsMobile())
            {
                return new SignUpTrustedResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken.Token,
                    User = _mapper.Map<UserBasicResponseDto>(user),
                    Roles = roles
                };
            }
            else
            {
                _httpContextManager.AddRefreshTokenToCookie(refreshToken.Token, refreshToken.ExpirationUtc);
                return new SignUpResponse
                {
                    AccessToken = accessToken,
                    User = _mapper.Map<UserBasicResponseDto>(user),
                    Roles = roles
                };
            }
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }


    [ExceptionHandler]
    [Validation(typeof(RefreshAuthRequest))]
    public async Task<RefreshAuthResponse> RefreshAuthAsync(RefreshAuthRequest refreshAuthRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            if (!_httpContextManager.IsMobile())
            {
                refreshAuthRequest.RefreshToken = _httpContextManager.GetRefreshTokenFromCookie();
            }

            var user = await _unitOfWork.Users.GetAsync(where: f => f.Id == refreshAuthRequest.UserId, cancellationToken: cancellationToken);
            if (user == null) throw new GeneralException("User cannot found for refresh auth!", description: $"Requester userId: {refreshAuthRequest.UserId}");

            string? ipAddress = _httpContextManager.GetClientIp();
            if (string.IsNullOrEmpty(ipAddress)) throw new GeneralException("Ip address could not readed for refresh auth.");

            DateTime nowOnUtc = DateTime.UtcNow;
            ICollection<RefreshToken>? refreshTokens = await _unitOfWork.RefreshTokens.GetAllAsync(where: f =>
                f.UserId == refreshAuthRequest.UserId &&
                f.TTL > 0 &&
                f.ExpirationUtc > nowOnUtc &&
                f.IpAddress.ToLowerInvariant().Trim() == ipAddress.ToLowerInvariant().Trim(),
                cancellationToken: cancellationToken
            );
            if (refreshTokens == null) throw new GeneralException("There is no available refresh token.");

            RefreshToken? refreshToken = refreshTokens.FirstOrDefault(f => f.Token.Trim() == refreshAuthRequest.RefreshToken);
            if (refreshToken == null) throw new GeneralException("There is no available refresh token.");

            refreshToken.Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            refreshToken.TTL -= 1;

            await _unitOfWork.RefreshTokens.DeleteAndSaveAsync(where: f => f.Id != refreshToken.Id && f.UserId == user.Id && f.IpAddress.Trim() == ipAddress.Trim(), cancellationToken);
            await _unitOfWork.RefreshTokens.UpdateAndSaveAsync(refreshToken, cancellationToken);

            IList<string> roles = await _userManager.GetRolesAsync(user);
            IList<Claim> claims = await GetClaimsAsync(user, roles);
            AccessToken accessToken = _tokenService.GenerateAccessToken(claims);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            if (_httpContextManager.IsMobile())
            {
                return new RefreshAuthTrustedResponse
                {
                    RefreshToken = refreshToken.Token,
                    AccessToken = accessToken,
                    User = _mapper.Map<UserBasicResponseDto>(user),
                };
            }
            else
            {
                _httpContextManager.AddRefreshTokenToCookie(refreshToken.Token, refreshToken.ExpirationUtc);
                return new RefreshAuthResponse
                {
                    AccessToken = accessToken,
                    User = _mapper.Map<UserBasicResponseDto>(user),
                };
            }
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }


    [Validation(typeof(LoginRequest))]
    public async Task LoginWebBaseAsync(LoginRequest loginRequest, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(loginRequest.Email);
        if (user == null) throw new BusinessException("The email address is not exist.", description: $"Requester email address: {loginRequest.Email}");

        var result = await _signInManager.PasswordSignInAsync(user, loginRequest.Password, isPersistent: true, lockoutOnFailure: false);

        if (result.IsLockedOut)
        {
            throw new BusinessException("Your account is locked.", description: $"Requester email address: {loginRequest.Email}");
        }
        else if (!result.Succeeded)
        {
            throw new BusinessException("Invalid login information..", description: $"Requester email address: {loginRequest.Email}");
        }
    }


    [Validation(typeof(SignUpRequest))]
    public async Task SignUpWebBaseAsync(SignUpRequest signUpRequest, CancellationToken cancellationToken = default)
    {
        var userExist = await _userManager.FindByEmailAsync(signUpRequest.Email);
        if (userExist != null) throw new BusinessException("The email address is already in use.", description: $"Requester email address: {signUpRequest.Email}");

        var user = _mapper.Map<User>(signUpRequest);
        user.UserName = $"{signUpRequest.Email}_{DateTime.UtcNow:yyyyMMddHHmmss}";

        var result = await _userManager.CreateAsync(user, signUpRequest.Password);
        if (!result.Succeeded) throw new GeneralException(string.Join("\n", result.Errors.Select(e => e.Description)), description: $"User cannot be created. Requester email: {signUpRequest.Email}");

        var roleResult = await _userManager.AddToRoleAsync(user, "User");
        if (!roleResult.Succeeded) throw new GeneralException("Failed to assign role.", description: $"Requester email address: {signUpRequest.Email}");

        var resultSignIn = await _signInManager.PasswordSignInAsync(user, signUpRequest.Password, isPersistent: true, lockoutOnFailure: false);

        if (resultSignIn.IsLockedOut)
        {
            throw new BusinessException("Your account is locked.", description: $"Requester email address: {signUpRequest.Email}");
        }
        else if (!resultSignIn.Succeeded)
        {
            throw new BusinessException("Invalid login information..", description: $"Requester email address: {signUpRequest.Email}");
        }
    }

    #region Helpers
    private async Task<IList<Claim>> GetClaimsAsync(User user, IList<string> roles)
    {
        List<Claim> claimList = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, $"{user.Name} {user.LastName}")
        };

        if (!string.IsNullOrEmpty(user.Email))
            claimList.Add(new Claim(ClaimTypes.Email, user.Email));

        IList<Claim>? persistentClaims = await _userManager.GetClaimsAsync(user);
        claimList.AddRange(persistentClaims);

        IEnumerable<Claim>? roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role));
        claimList.AddRange(roleClaims);

        return claimList;
    }
    #endregion
}