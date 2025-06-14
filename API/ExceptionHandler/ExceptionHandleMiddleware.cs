using Core.Enums;
using Core.ExceptionHandle.ProblemDetailModels;
using Core.Utils.ExceptionHandle.Exceptions;
using FluentValidation.Results;
using Newtonsoft.Json;

namespace API.ExceptionHandler;

public class ExceptionHandleMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandleMiddleware> _logger;

    public ExceptionHandleMiddleware(RequestDelegate next, ILogger<ExceptionHandleMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }


    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            await CatchExceptionAsync(context.Response, e);
        }
    }

    private Task CatchExceptionAsync(HttpResponse response, Exception exception)
    {
        response.ContentType = "application/problem+json";

        Type exceptionType = exception.GetType();

        if (exceptionType == typeof(ValidationRuleException)) return HandleValidationException(response, (ValidationRuleException)exception);
        if (exceptionType == typeof(DataAccessException)) return HandleDataAccessException(response, (DataAccessException)exception);
        if (exceptionType == typeof(BusinessException)) return HandleBusinessException(response, (BusinessException)exception);
        if (exceptionType == typeof(GeneralException)) return HandleGeneralException(response, (GeneralException)exception);

        return HandleOtherException(response, exception);
    }

    private Task HandleValidationException(HttpResponse response, ValidationRuleException exception)
    {
        _logger.LogError(
            $"\n\n------- ------- ------- Start ------- ------- ------- \n" +
            $"Type(Validation) \n" +
            $"Location: {exception.LocationName} \n" +
            $"Detail: {exception.Message} \n" +
            $"Description:{exception.Description} \n" +
            $"Parameters: {exception.Parameters} \n" +
            $"------- ------- ------- FINISH ------- ------- -------\n\n");

        response.StatusCode = StatusCodes.Status400BadRequest;
        IEnumerable<ValidationFailure> errors = exception.Errors;

        return response.WriteAsync(JsonConvert.SerializeObject(new ValidationProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Type = ProblemDetailTypes.Validation.ToString(),
            Title = "Validation error(s)",
            Detail = exception.Message,
            Errors = errors
        }));
    }

    private Task HandleBusinessException(HttpResponse response, BusinessException exception)
    {
        _logger.LogError(
            $"\n\n------- ------- ------- Start ------- ------- ------- \n" +
            $"Type(Business) \n" +
            $"Location: {exception.LocationName} \n" +
            $"Detail: {exception.Message} \n" +
            $"Description:{exception.Description} \n" +
            $"Parameters: {exception.Parameters} \n" +
            $"------- ------- ------- FINISH ------- ------- -------\n\n");

        response.StatusCode = StatusCodes.Status409Conflict;

        return response.WriteAsync(JsonConvert.SerializeObject(new BusinessProblemDetails
        {
            Status = StatusCodes.Status409Conflict,
            Type = ProblemDetailTypes.Business.ToString(),
            Title = "Business Workflow Exception",
            Detail = exception.Message
        }));
    }

    private Task HandleDataAccessException(HttpResponse response, DataAccessException exception)
    {
        _logger.LogError(exception,
            $"\n\n------- ------- ------- Start ------- ------- ------- \n" +
            $"Type(DataAccess) \n" +
            $"Location: {exception.LocationName} \n" +
            $"Detail: {exception.Message} \n" +
            $"Description:{exception.Description} \n" +
            $"Parameters: {exception.Parameters} \n" +
            $"------- ------- ------- FINISH ------- ------- -------\n\n");

        response.StatusCode = StatusCodes.Status500InternalServerError;

        return response.WriteAsync(JsonConvert.SerializeObject(new DataAccessProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Type = ProblemDetailTypes.DataAccess.ToString(),
            Title = "Data Access Exception",
            Detail = "An error occurred during the process",
        }));
    }

    private Task HandleGeneralException(HttpResponse response, GeneralException exception)
    {
        _logger.LogError(exception, 
            $"\n\n------- ------- ------- Start ------- ------- ------- \n" +
            $"Type(General) \n" +
            $"Location: {exception.LocationName} \n" +
            $"Detail: {exception.Message} \n" +
            $"Description:{exception.Description} \n" +
            $"Parameters: {exception.Parameters} \n" +
            $"------- ------- ------- FINISH ------- ------- -------\n\n");

        response.StatusCode = StatusCodes.Status500InternalServerError; // 500

        return response.WriteAsync(JsonConvert.SerializeObject(new Microsoft.AspNetCore.Mvc.ProblemDetails()
        {
            Status = StatusCodes.Status500InternalServerError,
            Type = ProblemDetailTypes.General.ToString(),
            Title = "General exception",
            Detail = "An error occurred during the process"
        }));
    }

    private Task HandleOtherException(HttpResponse response, Exception exception)
    {
        _logger.LogError(exception, 
            $"\n\n------- ------- ------- Start ------- ------- ------- \n" +
            $"Type(Others) \n" +
            $"Detail: {exception.Message} \n" +
            $"------- ------- ------- FINISH ------- ------- -------\n\n");

        response.StatusCode = StatusCodes.Status500InternalServerError; // 500

        return response.WriteAsync(JsonConvert.SerializeObject(new Microsoft.AspNetCore.Mvc.ProblemDetails()
        {
            Status = StatusCodes.Status500InternalServerError,
            Type = ProblemDetailTypes.General.ToString(),
            Title = "General exception",
            Detail = "An error occurred during the process"
        }));
    }
}