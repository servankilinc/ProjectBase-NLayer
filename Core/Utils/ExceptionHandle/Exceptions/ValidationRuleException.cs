using Core.Model;
using FluentValidation;
using FluentValidation.Results;

namespace Core.Utils.ExceptionHandle.Exceptions;

public class ValidationRuleException : ValidationException, IAppException
{
    public string? LocationName { get; set; }
    public string? Parameters { get; set; }
    public string? Description { get; set; }

    public ValidationRuleException(string message, string? locationName = default, string? parameters = default, string? description = default) : base(message)
    {
        LocationName = locationName;
        Parameters = parameters;
        Description = description;
    }

    public ValidationRuleException(IEnumerable<ValidationFailure> errors, string? locationName = default, string? parameters = default, string? description = default) : base(errors)
    {
        LocationName = locationName;
        Parameters = parameters;
        Description = description;
    }

    public ValidationRuleException(string message, IEnumerable<ValidationFailure> errors, string? locationName = default, string? parameters = default, string? description = default) : base(message, errors)
    {
        LocationName = locationName;
        Parameters = parameters;
        Description = description;
    }
}
