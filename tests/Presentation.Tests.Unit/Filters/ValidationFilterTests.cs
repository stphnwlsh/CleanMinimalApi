namespace CleanMinimalApi.Presentation.Tests.Unit.Filters;

using System.Threading.Tasks;
using CleanMinimalApi.Presentation.Filters;
using CleanMinimalApi.Presentation.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Shouldly;
using Xunit;

public class ValidationFilterTests
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
        var response = await filter.InvokeAsync(context, NextDelegate);

        // Assert
        response.ShouldBeSameAs(NextResult);
    }

    [Fact]
    public async Task InvokeAsync_ValidValue_BadRequest()
    {
        // Arrange
        var value = "Guid.NewGuid()";
        var filter = new ValidationFilter<Guid>(this.validator);
        var context = CreateFilterContext(value);

        // Act
        var response = await filter.InvokeAsync(context, NextDelegate);

        // Assert
        var result = response.ShouldBeOfType<BadRequest<string>>();

        result.StatusCode.ShouldBe(400);
        result.Value.ShouldBe("Unable to find parameters or body for validation");
    }

    [Fact]
    public async Task InvokeAsync_ValidValue_ValidationProblem()
    {
        // Arrange
        var value = Guid.Empty;
        var filter = new ValidationFilter<Guid>(this.validator);
        var context = CreateFilterContext(value);

        // Act
        var response = await filter.InvokeAsync(context, NextDelegate);

        // Assert
        var result = response.ShouldBeOfType<ProblemHttpResult>();

        result.StatusCode.ShouldBe(400);
        _ = result.ProblemDetails.ShouldNotBeNull();
        result.ProblemDetails.Title.ShouldBe("One or more validation errors occurred.");
    }

    #region Helper Methods

    private static DefaultEndpointFilterInvocationContext CreateFilterContext(object argument)
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
