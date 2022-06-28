namespace CleanMinimalApi.Presentation.Extensions;

using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;
using Application.Common.Exceptions;
using Errors;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

[ExcludeFromCodeCoverage]
public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        _ = app.UseExceptionHandler(appError => appError.Run(async context =>
        {
            var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

            if (contextFeature != null)
            {
                // Set the Http Status Code
                var statusCode = contextFeature.Error switch
                {
                    ValidationException ex => HttpStatusCode.BadRequest,
                    NotFoundException ex => HttpStatusCode.NotFound,
                    _ => HttpStatusCode.InternalServerError
                };

                // Prepare Generic Error
                var apiError = new ApiError(contextFeature.Error.Message, contextFeature.Error.InnerException?.Message, contextFeature.Error.StackTrace);

                // Set Response Details
                context.Response.StatusCode = (int)statusCode;
                context.Response.ContentType = "application/json";

                // Return the Serialized Generic Error
                await context.Response.WriteAsync(JsonSerializer.Serialize(apiError));
            }
        }));

        return app;
    }
}
