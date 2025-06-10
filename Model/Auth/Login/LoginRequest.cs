using FluentValidation;

namespace Model.Auth.Login;

public class LoginRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}


public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(b => b.Email).NotNull().EmailAddress().NotEmpty().EmailAddress();
        RuleFor(b => b.Password).NotNull().MinimumLength(6).NotEmpty();
    }
}
