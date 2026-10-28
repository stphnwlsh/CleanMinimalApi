namespace CleanMinimalApi.Presentation.Tests.Unit.Errors;

using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Common.Exceptions;
using CleanMinimalApi.Presentation.Errors;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Shouldly;
using Xunit;

public class ApplicationExceptionHandlerTests
{
    [Fact]
    public async Task TryHandleAsync_ShouldReturn_ValidationProblem()
    {
        // Arrange
        var exception = new ValidationException(
        [
            new ValidationFailure("Id", "A Review Id was not supplied."),
            new ValidationFailure("Stars", "A star rating must be between 1 and 5.")
        ]);

        // Act
        var (handled, httpContext, context) = await Handle(exception);

        // Assert
        handled.ShouldBeTrue();
        httpContext.Response.StatusCode.ShouldBe(StatusCodes.Status400BadRequest);

        var problemDetails = context.ProblemDetails.ShouldBeOfType<HttpValidationProblemDetails>();

        problemDetails.Status.ShouldBe(StatusCodes.Status400BadRequest);
        problemDetails.Errors.Count.ShouldBe(2);
        problemDetails.Errors["Id"].ShouldBe(["A Review Id was not supplied."]);
        problemDetails.Errors["Stars"].ShouldBe(["A star rating must be between 1 and 5."]);
        context.Exception.ShouldBe(exception);
    }

    [Fact]
    public async Task TryHandleAsync_ShouldReturn_NotFound()
    {
        // Arrange
        var exception = new NotFoundException("Expected Exception");

        // Act
        var (handled, httpContext, context) = await Handle(exception);

        // Assert
        handled.ShouldBeTrue();
        httpContext.Response.StatusCode.ShouldBe(StatusCodes.Status404NotFound);
        context.ProblemDetails.Status.ShouldBe(StatusCodes.Status404NotFound);
        context.ProblemDetails.Detail.ShouldBe("Expected Exception");
    }

    [Fact]
    public async Task TryHandleAsync_ShouldReturn_BadRequest()
    {
        // Arrange
        var exception = new BadHttpRequestException("Expected Exception");

        // Act
        var (handled, httpContext, context) = await Handle(exception);

        // Assert
        handled.ShouldBeTrue();
        httpContext.Response.StatusCode.ShouldBe(StatusCodes.Status400BadRequest);
        context.ProblemDetails.Status.ShouldBe(StatusCodes.Status400BadRequest);
        context.ProblemDetails.Detail.ShouldBe("Expected Exception");
    }

    [Fact]
    public async Task TryHandleAsync_ShouldReturn_Problem()
    {
        // Arrange
        var exception = new ArgumentException("Expected Exception");

        // Act
        var (handled, httpContext, context) = await Handle(exception);

        // Assert
        handled.ShouldBeTrue();
        httpContext.Response.StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);
        context.ProblemDetails.Status.ShouldBe(StatusCodes.Status500InternalServerError);
        context.ProblemDetails.Detail.ShouldBeNull();
    }

    private static async Task<(bool Handled, HttpContext HttpContext, ProblemDetailsContext Context)> Handle(Exception exception)
    {
        var problemDetailsService = Substitute.For<IProblemDetailsService>();
        ProblemDetailsContext context = null;

        _ = problemDetailsService
            .TryWriteAsync(Arg.Do<ProblemDetailsContext>(c => context = c))
            .Returns(ValueTask.FromResult(true));

        var handler = new ApplicationExceptionHandler(problemDetailsService);
        var httpContext = new DefaultHttpContext();

        var handled = await handler.TryHandleAsync(httpContext, exception, CancellationToken.None);

        return (handled, httpContext, context);
    }
}
