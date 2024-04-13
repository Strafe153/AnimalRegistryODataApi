using Domain.Exceptions;
using Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mime;
using ValidationException = FluentValidation.ValidationException;

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
				.Select(g =>
					new Error
					{
						 Property = g.Key,
						ErrorMessages = g.Select(v => v.ErrorMessage).Distinct()
					})
			: null;

		ProblemDetails problemDetails = new()
		{
			Type = rfcType,
			Title = exception.Message,
			Status = statusCodeAsInt,
			Instance = context.Request.Path,
			Detail = exception.Message
		};

		if (errors is not null
			&& problemDetails is FluentValidationProblemDetails fluentValidationProblemDetails)
		{
			fluentValidationProblemDetails.ValidationErrors = errors;
			return fluentValidationProblemDetails;
		}

		return problemDetails;
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