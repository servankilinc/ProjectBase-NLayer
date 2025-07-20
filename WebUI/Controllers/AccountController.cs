using Business.Abstract;
using Core.Utils.ExceptionHandle.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Model.Auth.Login;
using Model.Auth.SignUp;
using WebUI.Utils.ActionFilters;

namespace WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpGet]
        public IActionResult Login()
        {
            var model = new LoginRequest();
            return View(model);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilter<LoginRequest>))]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            try
            {
                await _authService.LoginWebBaseAsync(loginRequest);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                Type exType = ex.GetType();
                if (exType == typeof(BusinessException))
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "İşlem Sırasında Bir Sorun Oluştu. Lütfen Daha Sonra Tekrar Deneyiniz!");
                }
                return View(loginRequest);
            }
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilter<SignUpRequest>))]
        public async Task<IActionResult> SignUp(SignUpRequest signUpRequest)
        {
            try
            {
                await _authService.SignUpWebBaseAsync(signUpRequest);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                Type exType = ex.GetType();
                if (exType == typeof(BusinessException))
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "İşlem Sırasında Bir Sorun Oluştu. Lütfen Daha Sonra Tekrar Deneyiniz!");
                }
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
