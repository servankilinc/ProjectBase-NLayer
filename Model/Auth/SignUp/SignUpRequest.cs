﻿using FluentValidation;

namespace Model.Auth.SignUp;

public class SignUpRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
}

public class SignUpRequestValidator : AbstractValidator<SignUpRequest>
{
    public SignUpRequestValidator()
    {
        RuleFor(b => b.Email).NotNull().EmailAddress().NotEmpty().EmailAddress();
        RuleFor(b => b.Password).NotNull().MinimumLength(6).NotEmpty();

        RuleFor(b => b.FirstName).NotNull().MinimumLength(2).NotEmpty();
        RuleFor(b => b.LastName).NotNull().MinimumLength(2).NotEmpty().NotEqual(s => s.FirstName);
    }
}
