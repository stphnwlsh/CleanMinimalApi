namespace CleanMinimalApi.Application.Tests.Unit.Common.Behaviours;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Application.Authors.Entities;
using Application.Authors.Queries.GetAuthorById;
using Application.Common.Behaviours;
using FluentValidation;
using MediatR;
using NSubstitute;
using Shouldly;
using Xunit;

public class ValidationBehaviourTests
{
    [Fact]
    public async void Handle_ShouldValidate_NoErrors()
    {
        // Arrange
        var query = new GetAuthorByIdQuery { Id = Guid.NewGuid() };
        var validators = new List<IValidator<GetAuthorByIdQuery>> { new GetAuthorByIdValidator() };
        var handler = new ValidationBehaviour<GetAuthorByIdQuery, Author>(validators);
        var token = new CancellationTokenSource().Token;
        var deletgate = Substitute.For<RequestHandlerDelegate<Author>>();

        _ = deletgate().Returns(new Author
        {
            Id = Guid.Empty,
            FirstName = "Test",
            LastName = "Test"
        });

        // Act
        var result = await handler.Handle(query, deletgate, token);

        // Assert
        _ = result.ShouldNotBeNull();
    }

    [Fact]
    public void Handle_ShouldValidate_Errors()
    {
        // Arrange
        var query = new GetAuthorByIdQuery { Id = Guid.Empty };
        var validators = new List<IValidator<GetAuthorByIdQuery>> { new GetAuthorByIdValidator() };
        var handler = new ValidationBehaviour<GetAuthorByIdQuery, Author>(validators);
        var token = new CancellationTokenSource().Token;
        var deletgate = Substitute.For<RequestHandlerDelegate<Author>>();

        // Act
        var exception = Should.Throw<ValidationException>(async () => await handler.Handle(query, deletgate, token));

        // Assert
        _ = exception.ShouldNotBeNull();
        _ = exception.ShouldBeOfType<ValidationException>();
        exception.Errors.ShouldNotBeEmpty();

        var errors = exception.Errors.ToList();

        errors.Count.ShouldBe(1);
        errors[0].PropertyName.ShouldBe("Id");
    }
}
