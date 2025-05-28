using Core.Model;

namespace Core.Utils.ExceptionHandle.Exceptions;

public class GeneralException : Exception, IAppException
{
    public string? LocationName { get; set; }
    public string? Parameters { get; set; }
    public GeneralException(string message, string? locationName, string? parameters) : base(message)
    {
        LocationName = locationName;
        Parameters = parameters;
    }

    public GeneralException(string message, Exception innerException, string? locationName, string? parameters) : base(message, innerException)
    {
        LocationName = locationName;
        Parameters = parameters;
    }
}
