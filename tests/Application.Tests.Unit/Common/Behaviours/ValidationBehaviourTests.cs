namespace CleanMinimalApi.Application.Tests.Unit.Common.Behaviours;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using CleanMinimalApi.Application.Common.Behaviours;
using CleanMinimalApi.Application.Notes.Update;
using CleanMinimalApi.Domain.Entities.Notes;
using FluentValidation;
using MediatR;
using NSubstitute;
using Shouldly;
using Xunit;

[ExcludeFromCodeCoverage]
public class ValidationBehaviourTests
{
    [Fact]
    public async void Handle_ShouldValidate_NoErrors()
    {
        // Arrange
        var command = new UpdateNoteCommand(42, "Meaning of Life");
        var validators = new List<IValidator<UpdateNoteCommand>> { new UpdateNoteValidator() };
        var handler = new ValidationBehaviour<UpdateNoteCommand, Note>(validators);
        var token = new CancellationTokenSource().Token;
        var deletgate = Substitute.For<RequestHandlerDelegate<Note>>();

        _ = deletgate().Returns(new Note("test"));

        // Act
        var result = await handler.Handle(command, token, deletgate);

        // Assert
        _ = result.ShouldNotBeNull();
    }

    [Fact]
    public void Handle_ShouldValidate_Errors()
    {
        // Arrange
        var command = new UpdateNoteCommand(-1, "");
        var validators = new List<IValidator<UpdateNoteCommand>> { new UpdateNoteValidator() };
        var handler = new ValidationBehaviour<UpdateNoteCommand, Note>(validators);
        var token = new CancellationTokenSource().Token;
        var deletgate = Substitute.For<RequestHandlerDelegate<Note>>();

        // Act
        var exception = Should.Throw<ValidationException>(async () => await handler.Handle(command, token, deletgate));

        // Assert
        _ = exception.ShouldNotBeNull();
        _ = exception.ShouldBeOfType<ValidationException>();
        exception.Errors.ShouldNotBeEmpty();

        var errors = exception.Errors.ToList();

        errors.Count.ShouldBe(2);
        errors[0].PropertyName.ShouldBe("Id");
        errors[1].PropertyName.ShouldBe("Text");
    }
}
