namespace CleanMinimalApi.Presentation.Tests.Unit.Filters;

using System.Threading.Tasks;
using CleanMinimalApi.Presentation.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.Extensions.Logging;
using Xunit;
using FluentValidation;
using Shouldly;

public class LoggingFilterTests
{
    private readonly IValidator<Guid> validator;

    [Fact]
    public async Task InvokeAsync_ValidModel_ReturnsNext()
    {
        // Arrange
        var model = new Guid();
        var filter = new ValidationFilter<Guid>(this.validator);
        var context = CreateFilterContext(model);

        // Act
        var result = await filter.InvokeAsync(context, NextDelegate);

        // Assert
        result.ShouldBeSameAs(NextResult);
    }

    // Helper methods

    private static EndpointFilterInvocationContext CreateFilterContext(Guid argument)
    {
        var httpContext = new DefaultHttpContext();
        var filterContext = new DefaultEndpointFilterInvocationContext(httpContext, new List<object> { Guid.NewGuid() });

        return filterContext;
    }

    private static ValueTask<object> NextDelegate(EndpointFilterInvocationContext context)
    {
        return ValueTask.FromResult(NextResult);
    }

    private static readonly object NextResult = new();
}

public class LoggingFilter : IEndpointFilter
{
    private readonly ILogger<LoggingFilter> logger;

    public LoggingFilter(ILogger<LoggingFilter> logger)
    {
        this.logger = logger;
    }

    public async ValueTask<object> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        this.logger.LogInformation("LoggingFilter invoked.");
        return await next(context);
    }
}

//     public class ValidationFilterTests
//     {
//         private readonly IValidator<Guid> validator;

//         public ValidationFilterTests()
//         {
//             this.validator = new GenericIdentityValidator(); //Substitute.For<IValidator<Guid>>();
//         }

//         [Fact]
//         public async Task InvokeAsync_ValidModel_ReturnsNext()
//         {
//             // Arrange
//             var model = new Guid();
//             var filter = new ValidationFilter<Guid>(this.validator);
//             var context = CreateFilterContext(model);
//             var mockNextDelegate = new Func<EndpointFilterDelegate, Task<object>>(_ => Task.FromResult(new object()));

//             // Act
//             var result = await filter.InvokeAsync(context, EndpointFilterDelegate);

//             // Assert
//             result.ShouldBe("");
//         }

//         public delegate Task<object> EndpointFilterDelegate(EndpointFilterInvocationContext context);

//         // [Fact]
//         // public async Task InvokeAsync_InvalidModel_ReturnsValidationProblem()
//         // {
//         //     // Arrange
//         //     var model = new MyModel();
//         //     var validationFailures = new List<FluentValidation.Results.ValidationFailure>
//         //     {
//         //         new FluentValidation.Results.ValidationFailure("PropertyName", "Error Message")
//         //     };

//         //     _ = this.validator.ValidateAsync(Arg.Any<Guid>())
//         //         .Returns(new FluentValidation.Results.ValidationResult(validationFailures));

//         //     var filter = new ValidationFilter<Guid>(this.validator);
//         //     var context = CreateFilterContext(model);
//         //     var delegated = delegate Task<object> EndpointFilterDelegate(EndpointFilterInvocationContext context)

//         //     // Act
//         //     var result = await filter.InvokeAsync(context, null);

//         //     // Assert
//         //     _ = result.ShouldBeOfType<Microsoft.AspNetCore.Mvc.ValidationProblemDetails>();
//         //     var problemDetails = (Microsoft.AspNetCore.Mvc.ValidationProblemDetails)result;
//         //     problemDetails.Errors.Count.ShouldBe(1);
//         // }


//         //public delegate Task<object> EndpointFilterDelegate(EndpointFilterInvocationContext context);

//         [Fact]
//         public async Task InvokeAsync_ModelNotFound_ReturnsBadRequest()
//         {
//             // Arrange
//             var filter = new ValidationFilter<MyModel>(this.validator);
//             var context = CreateFilterContext(null);

//             // Act
//             var result = await filter.InvokeAsync(context, null);

//             // Assert
//             _ = result.ShouldBeOfType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>();
//         }

//         // Helper methods
//         private static DefaultEndpointFilterInvocationContext CreateFilterContext(object argument)
//         {
//             var httpContext = new DefaultHttpContext();
//             var routeData = new RouteData();
//             var endpoint = new RouteEndpoint(null, RoutePatternFactory.Parse(""), 0, null, null);
//             var filterContext = new DefaultEndpointFilterInvocationContext(httpContext, routeData, endpoint, new List<object> { argument });

//             return filterContext;
//         }

//         private delegate Task<object> NextDelegate(EndpointFilterInvocationContext context);
//         {
//             return Task.FromResult(NextResult);
//         }

//         private static readonly object NextResult = new();
//     }

//     public class MyModel { /* Define your model properties here */ }
// }
