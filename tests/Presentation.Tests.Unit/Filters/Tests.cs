// using CleanMinimalApi.Presentation.Filters;
// using FluentValidation;
// using Microsoft.AspNetCore.Http;
// using NSubstitute;
// using Xunit;

// namespace CleanMinimalApi.Tests.Filters
// {
//     public class ValidationFilterTests
//     {
//         [Fact]
//         public async void ValidationFilterFactory_CreatesFilterDelegate_WhenValidatorsExist2()
//         {


//         }



//             [Fact]
//         public async void ValidationFilterFactory_CreatesFilterDelegate_WhenValidatorsExist()
//         {
//             // Arrange
//             var methodInfo = typeof(TestController).GetMethod("TestMethodWithValidation").;
//             var serviceProvider = Substitute.For<IServiceProvider>();
//             var nextDelegate = Substitute.For<EndpointFilterDelegate>();
//             var context = EndpointFilterInvocationContext.Create((methodInfo, serviceProvider);

//             // Act
//             var filterDelegate = ValidationFilter.ValidationFilterFactory(context, nextDelegate);
//             var invocationContext = Substitute.For<EndpointFilterInvocationContext>();

//             // Assert
//             await filterDelegate(invocationContext);
//             nextDelegate.Received().Invoke(invocationContext);
//         }

//         [Fact]
//         public async void ValidationFilterFactory_ReturnsPassThruDelegate_WhenNoValidators()
//         {
//             // Arrange
//             var methodInfo = typeof(TestController).GetMethod("MethodWithoutValidator");
//             var serviceProvider = Substitute.For<IServiceProvider>();
//             var nextDelegate = Substitute.For<EndpointFilterDelegate>();
//             var context = new EndpointFilterFactoryContext(methodInfo, serviceProvider);

//             // Act
//             var filterDelegate = ValidationFilter.ValidationFilterFactory(context, nextDelegate);
//             var invocationContext = Substitute.For<EndpointFilterInvocationContext>();

//             // Assert
//             await filterDelegate(invocationContext);
//             nextDelegate.DidNotReceive().Invoke(invocationContext);
//         }

//         [Fact]
//         public async void Validate_ReturnsValidationProblem_WhenValidationFails()
//         {
//             // Arrange
//             var validationDescriptor = Substitute.For<ValidationDescriptor>();
//             validationDescriptor.Validator.Returns(Substitute.For<IValidator>());
//             var invocationContext = Substitute.For<EndpointFilterInvocationContext>();
//             var nextDelegate = Substitute.For<EndpointFilterDelegate>();

//             // Act
//             var result = await ValidationFilter.Validate(new[] { validationDescriptor }, invocationContext, nextDelegate);

//             // Assert
//             Assert.IsType<ValidationProblemDetails>(result);
//         }

//         [Fact]
//         public async void Validate_CallsNextDelegate_WhenValidationSucceeds()
//         {
//             // Arrange
//             var validationDescriptor = Substitute.For<ValidationDescriptor>();
//             validationDescriptor.Validator.Returns(Substitute.For<IValidator>());
//             var invocationContext = Substitute.For<EndpointFilterInvocationContext>();
//             var nextDelegate = Substitute.For<EndpointFilterDelegate>();

//             // Act
//             await ValidationFilter.Validate(new[] { validationDescriptor }, invocationContext, nextDelegate);

//             // Assert
//             nextDelegate.Received().Invoke(invocationContext);
//         }

//         // ... additional tests for GetValidators and other methods
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
// }
