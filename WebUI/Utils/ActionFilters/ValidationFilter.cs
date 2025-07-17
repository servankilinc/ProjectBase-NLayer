using Azure.Core;
using Core.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace WebUI.Utils.ActionFilters
{
    public class ValidationFilter<TModel> : ActionFilterAttribute
    {
        private readonly IEnumerable<IValidator<TModel>> _validators;
        public ValidationFilter(IEnumerable<IValidator<TModel>> validators) => _validators = validators;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!_validators.Any()) return;

            var model = (TModel?)context.ActionArguments.Values.FirstOrDefault(f => f.GetType() == typeof(TModel));
            if (model == null) return;

            IEnumerable<FluentValidation.Results.ValidationFailure> validationFailures = _validators
                .Select(validator => validator.Validate(model))
                .Where(result => !result.IsValid)
                .SelectMany(result => result.Errors)
                .ToList();

            if (!validationFailures.Any()) return;

            var isJsonRequest =
                context.HttpContext.Request.Headers["Accept"].ToString().Contains("application/json") ||
                context.HttpContext.Request.Headers["X-Requested-With"].ToString().Contains("XMLHttpRequest") ||
                context.HttpContext.Request.ContentType?.Contains("application/json") == true;


            if (isJsonRequest)
            {
                var problemDetails = new Core.Utils.ExceptionHandle.ProblemDetailModels.ValidationProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Type = ProblemDetailTypes.Validation.ToString(),
                    Title = "Validation error(s)",
                    Detail = "One or more validation errors occurred.",
                    Errors = validationFailures
                };

                context.Result = new BadRequestObjectResult(problemDetails);
            }
            else
            {
                foreach (var failure in validationFailures)
                {
                    context.ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                }

                string? action = context.ActionDescriptor.RouteValues["action"];
                context.Result = new ViewResult
                {
                    ViewName = action,
                    ViewData = new ViewDataDictionary(metadataProvider: new EmptyModelMetadataProvider(), modelState: context.ModelState)
                    {
                        Model = model
                    }
                };
            }
        }
    }
}
