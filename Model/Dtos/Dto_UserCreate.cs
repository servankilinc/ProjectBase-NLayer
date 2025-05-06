using Core.Model;
using FluentValidation;

namespace Model.Dtos;

public class Dto_UserCreate : IDto
{
    public string Name { get; set; } = null!;
    public string EmailAddres { get; set; } = null!;
    public string Password { get; set; } = null!;
    public DateOnly Date { get; set; }
}

public class Dto_UserCreateValidator : AbstractValidator<Dto_UserCreate>
{
    public Dto_UserCreateValidator()
    {
        RuleFor(v => v.Name).MinimumLength(3).WithMessage("Field must have a minimum 3 character");
        RuleFor(v => v.Name).MaximumLength(50).WithMessage("Field cannot exceed maximum length");
        RuleFor(v => v.EmailAddres).EmailAddress().WithMessage("Field must be a valid email address");
        RuleFor(v => v.Password).MinimumLength(6).WithMessage("Field must have a minimum 6 character");
        RuleFor(v => v.Date).NotEqual(DateOnly.MinValue).WithMessage("Field cannot be empty");
    }
}