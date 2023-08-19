namespace CleanMinimalApi.Presentation.Tests.Unit.Filters;

using System.Threading.Tasks;
using CleanMinimalApi.Presentation.Filters;
using CleanMinimalApi.Presentation.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Shouldly;
using Xunit;

public class LoggingFilterTests
{
    private readonly IValidator<Guid> validator = new GenericIdentityValidator();

    [Fact]
    public async Task InvokeAsync_ValidValue_ReturnsNext()
    {
        // Arrange
        var value = Guid.NewGuid();
        var filter = new ValidationFilter<Guid>(this.validator);
        var context = CreateFilterContext(value);

        // Act
        var result = await filter.InvokeAsync(context, NextDelegate);

        // Assert
        result.ShouldBeSameAs(NextResult);
    }

    [Fact]
    public async Task InvokeAsync_ValidValue_BadRequest()
    {
        // Arrange
        var value = "Guid.NewGuid()";
        var filter = new ValidationFilter<Guid>(this.validator);
        var context = CreateFilterContext(value);

        // Act
        var result = await filter.InvokeAsync(context, NextDelegate);

        // Assert
        _ = result.ShouldBeOfType<BadRequest<string>>();

        var expectedResult = result as BadRequest<string>;

        expectedResult.StatusCode.ShouldBe(400);
        expectedResult.Value.ShouldBe("Unable to find parameters or body for validation");
    }

    [Fact]
    public async Task InvokeAsync_ValidValue_ValidationProblem()
    {
        // Arrange
        var value = Guid.Empty;
        var filter = new ValidationFilter<Guid>(this.validator);
        var context = CreateFilterContext(value);

        // Act
        var result = await filter.InvokeAsync(context, NextDelegate);

        // Assert

        _ = result.ShouldBeOfType<ProblemHttpResult>();

        var expectedResult = result as ProblemHttpResult;

        expectedResult.StatusCode.ShouldBe(400);
        _ = expectedResult.ProblemDetails.ShouldNotBeNull();
        expectedResult.ProblemDetails.Title.ShouldBe("One or more validation errors occurred.");
    }

    #region Helper Methods

    private static EndpointFilterInvocationContext CreateFilterContext(object argument)
    {
        var httpContext = new DefaultHttpContext();
        var filterContext = new DefaultEndpointFilterInvocationContext(httpContext, argument);

        return filterContext;
    }

    private static ValueTask<object> NextDelegate(EndpointFilterInvocationContext context)
    {
        return ValueTask.FromResult(NextResult);
    }

    private static readonly object NextResult = new();

    #endregion Helper Methods
}
