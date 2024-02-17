// namespace CleanMinimalApi.Presentation.Tests.Unit.Filters;

// using System.Collections.Generic;
// using System.Reflection;
// using CleanMinimalApi.Presentation.Filters;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc.Filters;
// using Microsoft.Extensions.DependencyInjection;
// using NSubstitute;
// using Xunit;

// public class ValidationFilterTests
// {
//     [Fact]
//     public void Returns_pass_thru_delegate_when_no_validators_found()
//     {
//         // Arrange
//         var contextMock = Substitute.For<EndpointFilterFactoryContext>();
//         var nextMock = Substitute.For<EndpointFilterDelegate>();


//         contextMock.GetValidators(Arg.Any<MethodInfo>(), Arg.Any<IServiceProvider>())
//             .Returns(Enumerable.Empty<object>()); // No validators

//         // Act
//         var filterDelegate = ValidationFilterFactory(contextMock, nextMock);

//         // Assert
//         var invocationContextMock = Substitute.For<EndpointFilterInvocationContext>();
//         filterDelegate(invocationContextMock); // Call the pass-thru delegate
//         nextMock.Received().Invoke(invocationContextMock); // Ensure next delegate was called
//     }

//     [Fact]
//     public void Returns_validation_delegate_when_validators_are_found()
//     {
//         // Arrange
//         var contextMock = Substitute.For<EndpointFilterFactoryContext>();
//         var nextMock = Substitute.For<EndpointFilterDelegate>();
//         var validationDescriptors = new object[] { new object() }; // Dummy validators
//         contextMock.GetValidators(Arg.Any<MethodInfo>(), Arg.Any<IServiceProvider>())
//             .Returns(validationDescriptors);

//         // Act
//         var filterDelegate = ValidationFilterFactory(contextMock, nextMock);

//         // Assert
//         var invocationContextMock = Substitute.For<EndpointFilterInvocationContext>();
//         filterDelegate(invocationContextMock); // Call the validation delegate
//         Received.InOrder(() =>
//         {
//             Validate(validationDescriptors, invocationContextMock, nextMock); // Ensure validation is called
//             nextMock.Invoke(invocationContextMock); // Ensure next delegate is called after validation
//         });
//     }

//     // [Fact]
//     // public void ValidationFilterFactory_NoValidators_PassThrough()
//     // {
//     //     // Arrange
//     //     var context = CreateEndpointFilterFactoryContext(typeof(TestController).GetMethod("TestMethod"));
//     //     var nextDelegate = Substitute.For<EndpointFilterDelegate>();

//     //     // Act
//     //     var resultDelegate = ValidationFilter.ValidationFilterFactory(context, nextDelegate);

//     //     resultDelegate.

//     //     // Assert
//     //     Assert.Same(nextDelegate, resultDelegate);
//     // }

//     // [Fact]
//     // public void ValidationFilterFactory_WithValidators_ValidateAndPassThrough()
//     // {
//     //     // Arrange
//     //     var context = CreateEndpointFilterFactoryContext(typeof(TestController).GetMethod("TestMethodWithValidation"));
//     //     var nextDelegate = Substitute.For<EndpointFilterDelegate>();

//     //     // Act
//     //     var resultDelegate = ValidationFilter.ValidationFilterFactory(context, nextDelegate);

//     //     // Assert
//     //     Assert.NotSame(nextDelegate, resultDelegate);
//     // }

//     // [Fact]
//     // public void Validate_ValidArguments_NoValidationProblemResult()
//     // {
//     //     // Arrange
//     //     var validationDescriptors = new List<ValidationDescriptor>
//     //     {
//     //         new() { ArgumentIndex = 0, ArgumentType = typeof(string), Validator = Substitute.For<IValidator<string>>() }
//     //     };

//     //     var invocationContext = this.CreateInvocationContext("test");
//     //     var nextDelegate = Substitute.For<EndpointFilterDelegate>();

//     //     // Act
//     //     var result = await ValidationFilter.Validate(validationDescriptors, invocationContext, nextDelegate);

//     //     // Assert
//     //     Assert.Same(nextDelegate, result);
//     // }

//     // [Fact]
//     // public async Task Validate_InvalidArguments_ValidationProblemResult()
//     // {
//     //     // Arrange
//     //     var validationDescriptors = new List<ValidationDescriptor>
//     //     {
//     //         new() { ArgumentIndex = 0, ArgumentType = typeof(string), Validator = Substitute.For<IValidator<string>>() }
//     //     };

//     //     var validationResult = new ValidationResult();
//     //     var validator = Substitute.For<IValidator<string>>();
//     //     var invocationContext = this.CreateInvocationContext("test");
//     //     var nextDelegate = Substitute.For<EndpointFilterDelegate>();

//     //     validationResult.Errors.Add(new ValidationFailure("PropertyName", "Error message"));
//     //     _ = validator.ValidateAsync(Arg.Any<ValidationContext<string>>()).Returns(validationResult);


//     //     // Act
//     //     var result = await ValidationFilter..Validate(validationDescriptors, invocationContext, nextDelegate);

//     //     // Assert
//     //     Assert.IsType<ValidationProblemObjectResult>(result);
//     // }

//     private static EndpointFilterFactoryContext CreateEndpointFilterFactoryContext(MethodInfo methodInfo)
//     {
//         var serviceProvider = new ServiceCollection().BuildServiceProvider();

//         return new EndpointFilterFactoryContext
//         {
//             MethodInfo = methodInfo,           // this is the original handler
//             ApplicationServices = serviceProvider
//         };
//     }

//     private EndpointFilterInvocationContext CreateInvocationContext(object argument)
//     {
//         var arguments = new object[] { argument };

//         return new DefaultEndpointFilterInvocationContext(
//             new DefaultHttpContext(),
//             new List<IFilterMetadata>(),
//             arguments
//         );
//     }

//     public class TestController
//     {
//         public void TestMethod(string input)
//         {
//             // Method with validation attribute
//         }

//         public void TestMethodWithValidation([Validate] string input)
//         {
//             // Method with validation attribute
//         }

//         private static readonly object NextResult = new();

//         private static ValueTask<object> NextDelegate(EndpointFilterInvocationContext context)
//         {
//             return invocationContext => next(invocationContext);
//         }

//     }

//     // #region Helper Methods

//     // private static DefaultEndpointFilterInvocationContext CreateFilterContext(object argument)
//     // {
//     //     var httpContext = new DefaultHttpContext();
//     //     var filterContext = new DefaultEndpointFilterInvocationContext(httpContext, argument);

//     //     return filterContext;
//     // }

//     // private static ValueTask<object> NextDelegate(EndpointFilterInvocationContext context)
//     // {
//     //     return ValueTask.FromResult(NextResult);
//     // }

//     // private static readonly object NextResult = new();

//     // #endregion Helper Methods
// }






// // using System.Threading.Tasks;
// // using CleanMinimalApi.Presentation.Filters;
// // using CleanMinimalApi.Presentation.Validators;
// // using FluentValidation;
// // using Microsoft.AspNetCore.Http;
// // using Microsoft.AspNetCore.Http.HttpResults;
// // using Shouldly;
// // using Xunit;

// // public class ValidationFilterTests
// // {
// //     private readonly IValidator<Guid> validator = new GenericIdentityValidator();

// //     [Fact]
// //     public async Task InvokeAsync_ValidValue_ReturnsNext()
// //     {
// //         // Arrange
// //         var value = Guid.NewGuid();
// //         var filter = new ValidationFilter<Guid>(this.validator);
// //         var context = CreateFilterContext(value);

// //         // Act
// //         var response = await filter.InvokeAsync(context, NextDelegate);

// //         // Assert
// //         response.ShouldBeSameAs(NextResult);
// //     }

// //     [Fact]
// //     public async Task InvokeAsync_ValidValue_BadRequest()
// //     {
// //         // Arrange
// //         var value = "Guid.NewGuid()";
// //         var filter = new ValidationFilter<Guid>(this.validator);
// //         var context = CreateFilterContext(value);

// //         // Act
// //         var response = await filter.InvokeAsync(context, NextDelegate);

// //         // Assert
// //         var result = response.ShouldBeOfType<BadRequest<string>>();

// //         result.StatusCode.ShouldBe(400);
// //         result.Value.ShouldBe("Unable to find parameters or body for validation");
// //     }

// //     [Fact]
// //     public async Task InvokeAsync_ValidValue_ValidationProblem()
// //     {
// //         // Arrange
// //         var value = Guid.Empty;
// //         var filter = new ValidationFilter<Guid>(this.validator);
// //         var context = CreateFilterContext(value);

// //         // Act
// //         var response = await filter.InvokeAsync(context, NextDelegate);

// //         // Assert
// //         var result = response.ShouldBeOfType<ProblemHttpResult>();

// //         result.StatusCode.ShouldBe(400);
// //         _ = result.ProblemDetails.ShouldNotBeNull();
// //         result.ProblemDetails.Title.ShouldBe("One or more validation errors occurred.");
// //     }


// // }
