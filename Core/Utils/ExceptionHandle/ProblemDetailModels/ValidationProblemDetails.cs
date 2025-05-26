using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Core.ExceptionHandle.ProblemDetailModels;

public class ValidationProblemDetails : ProblemDetails
{
    public IEnumerable<ValidationFailure>? Errors { get; set; }

    public override string ToString() => JsonConvert.SerializeObject(this);
}