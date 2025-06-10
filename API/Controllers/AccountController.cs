using Business.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Auth.Login;
using Model.Auth.RefreshAuth;
using Model.Auth.SignUp;

namespace API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAuthService _authService;
    public AccountController(IAuthService authService) => _authService = authService;


    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> SignUp(SignUpRequest request)
    {
        var result = await _authService.SignUpAsync(request);

        return Ok(result);
    }


    [HttpPost]
    public async Task<IActionResult> RefreshAuth(RefreshAuthRequest request)
    {
        var result = await _authService.RefreshAuthAsync(request);

        return Ok(result);
    }

}