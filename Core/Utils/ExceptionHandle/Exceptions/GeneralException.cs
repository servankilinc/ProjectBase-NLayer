using Core.Model;

namespace Core.Utils.ExceptionHandle.Exceptions;

public class GeneralException : Exception, IAppException
{
    public string? LocationName { get; set; }
    public string? Parameters { get; set; }
    public string? Description { get; set; }

    public GeneralException(string message, string? locationName = default, string? parameters = default, string? description = default) : base(message)
    {
        LocationName = locationName;
        Parameters = parameters;
        Description = description;
    }

    public GeneralException(string message, Exception innerException, string? locationName = default, string? parameters = default, string? description = default) : base(message, innerException)
    {
        LocationName = locationName;
        Parameters = parameters;
        Description = description;
    }
}
