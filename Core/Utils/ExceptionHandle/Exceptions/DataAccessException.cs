namespace Core.Utils.ExceptionHandle.Exceptions;

public class DataAccessException : Exception
{
    public DataAccessException(string message) : base(message)
    {
    }
}