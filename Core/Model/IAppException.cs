namespace Core.Model;

public interface IAppException
{
    string? LocationName { get; set; }
    string? Parameters { get; set; }
    string? Description { get; set; }
}
