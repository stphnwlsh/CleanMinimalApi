namespace CleanMinimalApi.Presentation.Errors;

using CleanMinimalApi.Application.Common.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Maps exceptions raised by the Application layer to RFC 9457 problem details responses.
/// </summary>
public class ApplicationExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var problemDetails = exception switch
        {
            ValidationException validationException => new HttpValidationProblemDetails(
                validationException.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).Distinct().ToArray()))
            {
                Status = StatusCodes.Status400BadRequest
            },
            NotFoundException => new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Detail = exception.Message
            },
            BadHttpRequestException badHttpRequestException => new ProblemDetails
            {
                Status = badHttpRequestException.StatusCode,
                Detail = badHttpRequestException.Message
            },
            _ => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError
            }
        };

        httpContext.Response.StatusCode = problemDetails.Status!.Value;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails,
            Exception = exception
        });
    }
}
