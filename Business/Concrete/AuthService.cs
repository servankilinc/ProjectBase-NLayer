using AutoMapper;
using Business.Abstract;
using Business.Utils.TokenService;
using Core.Utils.Auth;
using Core.Utils.CrossCuttingConcerns;
using Core.Utils.ExceptionHandle.Exceptions;
using Core.Utils.RequestInfoProvider;
using DataAccess.UoW;
using Microsoft.AspNetCore.Identity;
using Model.Auth.Login;
using Model.Auth.RefreshAuth;
using Model.Auth.SignUp;
using Model.Entities;
using System.Security.Cryptography;

namespace Business.Concrete;

[ExceptionHandler]
public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly UserManager<User> _userManager;
    private readonly RequestInfoProvider _requestInfoProvider;
    private readonly IMapper _mapper;
    public AuthService(
        IUnitOfWork unitOfWork,
        ITokenService tokenService,
        UserManager<User> userManager,
        RequestInfoProvider requestInfoProvider,
        IMapper mapper
    )
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _userManager = userManager;
        _requestInfoProvider = requestInfoProvider;
        _mapper = mapper;
    }


    [Validation(typeof(LoginRequest))]
    public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken = default)
    {
        User? user = await _userManager.FindByEmailAsync(loginRequest.Email);
        if (user == null) throw new BusinessException("The email address is already in use", description: $"Requester email address: {loginRequest.Email}");

        bool isPasswordValid = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
        if (!isPasswordValid) throw new BusinessException("Password does not correct", description: $"Requester email address: {loginRequest.Email}");

        IList<string> roles = await _userManager.GetRolesAsync(user);

        AccessToken accessToken = await _tokenService.CreateAccessToken(user, roles);
        RefreshToken refreshToken = _tokenService.CreateRefreshToken(user);

        string? ipAddress = _requestInfoProvider.GetClientIp();
        if (ipAddress == null) throw new GeneralException("Ip address could not found for login.", description: $"Requester email address: {loginRequest.Email}");

        await _unitOfWork.RefreshTokens.DeleteAndSaveAsync(where: f => f.UserId == user.Id && f.IpAddress.Trim() == ipAddress.Trim());
        refreshToken = await _unitOfWork.RefreshTokens.AddAndSaveAsync(refreshToken);

        return new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            User = user,
            Roles = roles
        };
    }


    [Validation(typeof(SignUpRequest))]
    public async Task<SignUpResponse> SignUpAsync(SignUpRequest signUpRequest, CancellationToken cancellationToken = default)
    {
        bool emailValidation = await _unitOfWork.Users.IsExistAsync(where: f => f.NormalizedEmail == signUpRequest.Email.ToUpperInvariant()); //_userManager.FindByEmailAsync(signUpRequest.Email);
        if (emailValidation) throw new BusinessException("The email address is already in use.", description: $"Requester email address: {signUpRequest.Email}");

        User userToCreate = _mapper.Map<User>(signUpRequest);
        userToCreate.UserName = $"{signUpRequest.Email}_{DateTime.UtcNow.ToString()}";

        var result = await _userManager.CreateAsync(userToCreate);
        if (!result.Succeeded) throw new GeneralException(string.Join(separator: $" ", value: result.Errors.Select(e => e.Description).ToArray()), description: $"User cannot create, requester email address: {signUpRequest.Email}");

        await _userManager.AddToRoleAsync(userToCreate, "User");

        User? createdUser = await _userManager.FindByEmailAsync(signUpRequest.Email);
        if (createdUser == null) throw new GeneralException("Created user cannot found after signup process!", description: $"Requester email address: {signUpRequest.Email}");

        IList<string> roles = await _userManager.GetRolesAsync(createdUser);

        AccessToken accessToken = await _tokenService.CreateAccessToken(createdUser, roles);
        RefreshToken refreshToken = _tokenService.CreateRefreshToken(createdUser);

        refreshToken = await _unitOfWork.RefreshTokens.AddAndSaveAsync(refreshToken);

        return new SignUpResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            User = createdUser,
            Roles = roles
        };
    }


    [Validation(typeof(RefreshAuthRequest))]
    public async Task<RefreshAuthResponse> RefreshAuthAsync(RefreshAuthRequest refreshAuthRequest, CancellationToken cancellationToken = default)
    {
        User? user = await _unitOfWork.Users.GetAsync(where: f => f.Id == refreshAuthRequest.UserId);
        if (user == null) throw new GeneralException("User cannot found for refresh auth!", description: $"Requester userId: {refreshAuthRequest.UserId}");

        string? ipAddress = _requestInfoProvider.GetClientIp();
        if (ipAddress == null) throw new Exception("Ip address could not found for refresh auth.");

        DateTime dateTimeNow = DateTime.UtcNow;
        RefreshToken? refreshToken = await _unitOfWork.RefreshTokens.GetAsync(where: f =>
            f.UserId == refreshAuthRequest.UserId &&
            f.TTL > 0 &&
            f.ExpirationUtc > dateTimeNow &&
            f.IpAddress.Trim() == ipAddress.Trim()
        );
        if (refreshToken == null) throw new Exception("There is no refresh token available.");

        refreshToken.Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        refreshToken.TTL -= 1;

        refreshToken = await _unitOfWork.RefreshTokens.UpdateAndSaveAsync(refreshToken, cancellationToken);

        IList<string> roles = await _userManager.GetRolesAsync(user);
        AccessToken accessToken = await _tokenService.CreateAccessToken(user, roles);

        return new RefreshAuthResponse
        {
            RefreshToken = refreshToken,
            AccessToken = accessToken
        };
    }
}
