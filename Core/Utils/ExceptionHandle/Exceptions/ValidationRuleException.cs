using Core.Model;
using FluentValidation;
using FluentValidation.Results;

namespace Core.Utils.ExceptionHandle.Exceptions;

public class ValidationRuleException : ValidationException, IAppException
{
    public string? LocationName { get; set; }
    public string? Parameters { get; set; }

    public ValidationRuleException(string message, string? locationName, string? parameters) : base(message)
    {
        LocationName = locationName;
        Parameters = parameters;
    }

    public ValidationRuleException(IEnumerable<ValidationFailure> errors, string? locationName, string? parameters) : base(errors)
    {
        LocationName = locationName;
        Parameters = parameters;
    }

    public ValidationRuleException(string message, IEnumerable<ValidationFailure> errors, string? locationName, string? parameters) : base(message, errors)
    {
        LocationName = locationName;
        Parameters = parameters;
    }
}
