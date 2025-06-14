using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Core.ExceptionHandle.ProblemDetailModels;

public class ValidationProblemDetails : ProblemDetails
{
    public IEnumerable<ValidationFailure>? Errors { get; set; }
}