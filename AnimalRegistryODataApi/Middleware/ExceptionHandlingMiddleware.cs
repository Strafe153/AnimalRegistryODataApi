using Domain.Exceptions;
using Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;
using System.Text.Json;

namespace AnimalRegistryODataApi.Middleware;

public class ExceptionHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = GetHttpStatusCode(exception);
        var statusCodeAsInt = (int)statusCode;

        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = statusCodeAsInt;

        ProblemDetails problemDetails = new()
        {
            Type = GetRFCType(statusCode),
            Status = statusCodeAsInt,
            Instance = context.Request.Path,
            Detail = exception.Message
        };

        var json = JsonSerializer.Serialize(problemDetails);

        return context.Response.WriteAsync(json);
    }

    private static HttpStatusCode GetHttpStatusCode(Exception exception) =>
        exception switch
        {
            NullReferenceException => HttpStatusCode.NotFound,
            OperationFailedException => HttpStatusCode.BadRequest,
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