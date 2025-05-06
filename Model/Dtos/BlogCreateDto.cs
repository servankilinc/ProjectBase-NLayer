using Core.Model;
using FluentValidation;

namespace Model.Dtos;

public class BlogCreateDto : IDto
{
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string ImgSource { get; set; } = null!;
    public Guid AuthorId { get; set; }
}


public class BlogCreateDtoValidator: AbstractValidator<BlogCreateDto>
{
    public BlogCreateDtoValidator()
    {
        RuleFor(v => v.AuthorId).NotEqual(Guid.Empty).WithMessage("Field must be a valid guid");
        RuleFor(v => v.AuthorId).NotNull().WithMessage("Field cannot be null");
        RuleFor(v => v.Title).NotEmpty().WithMessage("Field cannot be empty");
        RuleFor(v => v.Title).MinimumLength(4).WithMessage("Field must have a minimum 4 character");
        RuleFor(v => v.Content).NotEmpty().WithMessage("Field cannot be empty");
        RuleFor(v => v.ImgSource).NotEmpty().WithMessage("Field cannot be empty"); 
    }
}