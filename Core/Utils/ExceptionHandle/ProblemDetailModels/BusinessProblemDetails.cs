using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Core.ExceptionHandle.ProblemDetailModels;

public class BusinessProblemDetails : ProblemDetails
{
    public override string ToString() => JsonSerializer.Serialize(this);
}