using Core.Enums;
using Core.Utils.ExceptionHandle.Exceptions;
using Core.Utils.ExceptionHandle.ProblemDetailModels;
using FluentValidation.Results;
using Newtonsoft.Json;
using Serilog;
using WebUI.Utils.Extensions;

namespace WebUI.ExceptionHandler;

public class ExceptionHandleMiddleware
{
    private readonly RequestDelegate _next;
    public ExceptionHandleMiddleware(RequestDelegate next) => _next = next;


    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            if (context.IsJsonRequest())
            {
                await CatchJsonExceptionAsync(context.Response, e);
            }
            else 
            {
                CatchPageException(context.Response, e);
            }
        }
    }


    private void CatchPageException(HttpResponse response, Exception exception)
    {
        //response.Clear();

        Type exceptionType = exception.GetType();

        if (exceptionType == typeof(ValidationRuleException))
        {
            var validException = (ValidationRuleException)exception;
            Log.ForContext("Target", "Validation").Error(
                $"\n\n------- ------- ------- Start ------- ------- ------- \n" +
                $"Type(Validation) \n" +
                $"Location: {validException.LocationName} \n" +
                $"Detail: {validException.Message} \n" +
                $"Description:{validException.Description} \n" +
                $"Parameters: {validException.Parameters} \n" +
                $"------- ------- ------- FINISH ------- ------- -------\n\n");

            response.Redirect("/Error/InvalidProcess");
        }
        else if (exceptionType == typeof(BusinessException))
        {
            var businessException = (BusinessException)exception;
            Log.ForContext("Target", "Business").Error(
                $"\n\n------- ------- ------- Start ------- ------- ------- \n" +
                $"Type(Business) \n" +
                $"Location: {businessException.LocationName} \n" +
                $"Detail: {businessException.Message} \n" +
                $"Description:{businessException.Description} \n" +
                $"Parameters: {businessException.Parameters} \n" +
                $"Exception Raw: \n\n{businessException.ToString()} \n" +
                $"------- ------- ------- FINISH ------- ------- -------\n\n");

            response.Redirect("/Error/InvalidProcess");
        }
        else if (exceptionType == typeof(GeneralException))
        {
            var generalException = (GeneralException)exception;
            Log.ForContext("Target", "Application").Error(
                $"\n\n------- ------- ------- Start ------- ------- ------- \n" +
                $"Type(General) \n" +
                $"Location: {generalException.LocationName} \n" +
                $"Detail: {generalException.Message} \n" +
                $"Description:{generalException.Description} \n" +
                $"Parameters: {generalException.Parameters} \n" +
                $"Exception Raw: \n\n{generalException.ToString()} \n" +
                $"------- ------- ------- FINISH ------- ------- -------\n\n");

            response.Redirect("/Error/InternalServer");
        }
        else if (exceptionType == typeof(DataAccessException))
        {
            var dataAccessException = (DataAccessException)exception;
            Log.ForContext("Target", "DataAccess").Error(
                $"\n\n------- ------- ------- Start ------- ------- ------- \n" +
                $"Type(DataAccess) \n" +
                $"Location: {dataAccessException.LocationName} \n" +
                $"Detail: {dataAccessException.Message} \n" +
                $"Description:{dataAccessException.Description} \n" +
                $"Parameters: {dataAccessException.Parameters} \n" +
                $"Exception Raw: \n\n{dataAccessException.ToString()} \n" +
                $"------- ------- ------- FINISH ------- ------- -------\n\n");

            response.Redirect("/Error/InternalServer");
        }
        else if (response.StatusCode == 404 && !response.HasStarted)
        {
            Log.Error(
                $"\n\n------- ------- ------- Start ------- ------- ------- \n" +
                $"Type(Not Found) \n" +
                $"Detail: {exception.Message} \n" +
                $"Exception Raw: \n\n{exception.ToString()} \n" +
                $"------- ------- ------- FINISH ------- ------- -------\n\n");
            response.Redirect("/Error/NotFound");
        }
        else
        {
            Log.Error(
                $"\n\n------- ------- ------- Start ------- ------- ------- \n" +
                $"Type(Others) \n" +
                $"Detail: {exception.Message} \n" +
                $"Exception Raw: \n\n{exception.ToString()} \n" +
                $"------- ------- ------- FINISH ------- ------- -------\n\n");

            response.Redirect("/Error/InternalServer");
        }
    }

    private Task CatchJsonExceptionAsync(HttpResponse response, Exception exception)
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
        Log.ForContext("Target", "Validation").Error(
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
        Log.ForContext("Target", "Business").Error(
            $"\n\n------- ------- ------- Start ------- ------- ------- \n" +
            $"Type(Business) \n" +
            $"Location: {exception.LocationName} \n" +
            $"Detail: {exception.Message} \n" +
            $"Description:{exception.Description} \n" +
            $"Parameters: {exception.Parameters} \n" +
            $"Exception Raw: \n\n{exception.ToString()} \n" +
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
        Log.ForContext("Target", "DataAccess").Error(
            $"\n\n------- ------- ------- Start ------- ------- ------- \n" +
            $"Type(DataAccess) \n" +
            $"Location: {exception.LocationName} \n" +
            $"Detail: {exception.Message} \n" +
            $"Description:{exception.Description} \n" +
            $"Parameters: {exception.Parameters} \n" +
            $"Exception Raw: \n\n{exception.ToString()} \n" +
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
        Log.ForContext("Target", "Application").Error(
            $"\n\n------- ------- ------- Start ------- ------- ------- \n" +
            $"Type(General) \n" +
            $"Location: {exception.LocationName} \n" +
            $"Detail: {exception.Message} \n" +
            $"Description:{exception.Description} \n" +
            $"Parameters: {exception.Parameters} \n" +
            $"Exception Raw: \n\n{exception.ToString()} \n" +
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
        Log.Error(
            $"\n\n------- ------- ------- Start ------- ------- ------- \n" +
            $"Type(Others) \n" +
            $"Detail: {exception.Message} \n" +
            $"Exception Raw: \n\n{exception.ToString()} \n" +
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