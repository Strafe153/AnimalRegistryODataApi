using Core.Exceptions;
using Core.Shared;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using ValidationException = FluentValidation.ValidationException;

namespace AnimalRegistryODataApi.Middleware;

public class ExceptionsMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = GetHttpStatusCode(exception);
        var statusCodeAsInt = (int)statusCode;

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCodeAsInt;

        var problemDetails = GetProblemDetails(context, exception, statusCode, statusCodeAsInt);
        var json = JsonConvert.SerializeObject(problemDetails);

        return context.Response.WriteAsync(json);
    }

    private static ProblemDetails GetProblemDetails(
        HttpContext context,
        Exception exception,
        HttpStatusCode statusCode,
        int statusCodeAsInt)
    {
        var rfcType = GetRFCType(statusCode);
        var errors = exception is ValidationException validationException
           ? validationException
               .Errors
               .GroupBy(v => v.PropertyName)
               .Select(g => new Error()
               {
                   Property = g.Key,
                   ErrorMessages = g.Select(v => v.ErrorMessage).Distinct()
               })
           : null;

        if (errors is null)
        {
            var problemDetails = new ProblemDetails()
            {
                Type = rfcType,
                Title = exception.Message,
                Status = statusCodeAsInt,
                Instance = context.Request.Path,
                Detail = exception.Message
            };

            return problemDetails;
        }
        else
        {
            var problemDetails = new FluentValidationProblemDetails()
            {
                Type = rfcType,
                Title = exception.Message,
                Status = statusCodeAsInt,
                Instance = context.Request.Path,
                Detail = exception.Message,
                ValidationErrors = errors
            };

            return problemDetails;
        }
    }

    private static HttpStatusCode GetHttpStatusCode(Exception exception) =>
        exception switch
        {
            NullReferenceException => HttpStatusCode.NotFound,
            OperationFailedException => HttpStatusCode.BadRequest,
            ValidationException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError
        };

    private static string GetRFCType(HttpStatusCode statusCode) =>
        statusCode switch
        {
            HttpStatusCode.NotFound => RFCType.NotFound,
            HttpStatusCode.BadRequest => RFCType.BadRequest,
            _ => RFCType.InternalServerError
        };
}