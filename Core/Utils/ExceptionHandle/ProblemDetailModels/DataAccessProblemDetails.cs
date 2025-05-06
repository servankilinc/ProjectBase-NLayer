using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Core.ExceptionHandle.ProblemDetailModels;

public class DataAccessProblemDetails : ProblemDetails
{
    public override string ToString() => JsonSerializer.Serialize(this);
}
