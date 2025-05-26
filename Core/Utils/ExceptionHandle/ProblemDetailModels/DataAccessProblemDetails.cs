using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Core.ExceptionHandle.ProblemDetailModels;

public class DataAccessProblemDetails : ProblemDetails
{
    public override string ToString() => JsonConvert.SerializeObject(this);
}
