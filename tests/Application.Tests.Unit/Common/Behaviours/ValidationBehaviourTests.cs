namespace CleanMinimalApi.Application.Tests.Unit.Common.Behaviours;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Authors.Entities;
using Application.Authors.Queries.GetAuthorById;
using Application.Common.Behaviours;
using FluentValidation;
using Mediator;
using NSubstitute;
using Shouldly;
using Xunit;

public class ValidationBehaviourTests
{
    [Fact]
    public async Task Handle_ShouldValidate_NoErrors()
    {
        // Arrange
        var query = new GetAuthorByIdQuery { Id = Guid.NewGuid() };
        var validators = new List<IValidator<GetAuthorByIdQuery>> { new GetAuthorByIdValidator() };
        var handler = new ValidationBehaviour<GetAuthorByIdQuery, Author>(validators);
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;
        var next = Substitute.For<MessageHandlerDelegate<GetAuthorByIdQuery, Author>>();

        _ = next(query, token).Returns(new Author(Guid.Empty, "Test", "Test", []));

        // Act
        var result = await handler.Handle(query, next, token);

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
        using var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;
        var next = Substitute.For<MessageHandlerDelegate<GetAuthorByIdQuery, Author>>();

        // Act
        var exception = Should.Throw<ValidationException>(async () => await handler.Handle(query, next, token));

        // Assert
        _ = exception.ShouldNotBeNull();
        _ = exception.ShouldBeOfType<ValidationException>();
        exception.Errors.ShouldNotBeEmpty();

        var errors = exception.Errors.ToList();

        errors.Count.ShouldBe(1);
        errors[0].PropertyName.ShouldBe("Id");
    }
}
