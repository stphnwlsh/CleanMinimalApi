namespace CleanMinimalApi.Infrastructure.Tests.Unit.Persistance.HardCoded;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using CleanMinimalApi.Application.Common.Enums;
using CleanMinimalApi.Application.Common.Exceptions;
using CleanMinimalApi.Domain.Entities.Notes;
using CleanMinimalApi.Infrastructure.Persistance.HardCoded;
using NSubstitute;
using Shouldly;
using Xunit;

[ExcludeFromCodeCoverage]
public class HardCodedNotesContextTests
{
    #region Create

    [Fact]
    public async void Create_ShouldReturn_Note()
    {
        // Arrange
        var dataSource = Substitute.For<HardCodedNotesDataSource>();
        var context = new HardCodedNotesContext(dataSource);
        var token = new CancellationTokenSource().Token;
        var id = 42;
        var text = "Meaning of Life";

        _ = dataSource.NewNote(text).ReturnsForAnyArgs(new Note(id, text));

        // Act
        var result = await context.Create(text, token);

        // Assert
        _ = result.ShouldNotBeNull();
        _ = result.ShouldBeOfType<Note>();
        result.Id.ShouldBe(id);
        result.Text.ShouldBe(text);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Create_ShouldReturn_ActionFailedException(string text)
    {
        // Arrange
        var dataSource = Substitute.For<HardCodedNotesDataSource>();
        var context = new HardCodedNotesContext(dataSource);
        var token = new CancellationTokenSource().Token;

        // Act
        var exception = Should.Throw<ActionFailedException>(async () => await context.Create(text, token));

        // Assert
        _ = exception.ShouldNotBeNull();
        _ = exception.ShouldBeOfType<ActionFailedException>();
        exception.Entity.ShouldBe(EntityType.Notes);
        exception.Action.ShouldBe(ActionType.Create);
        exception.Message.ShouldBe("Note failed to Create in the Data Source");
    }

    #endregion Create

    #region Delete

    [Fact]
    public async void Delete_ShouldReturn_True()
    {
        // Arrange
        var dataSource = Substitute.For<HardCodedNotesDataSource>();
        var context = new HardCodedNotesContext(dataSource);
        var token = new CancellationTokenSource().Token;

        _ = dataSource.NewBool().ReturnsForAnyArgs(true);

        // Act
        var result = await context.Delete(10, token);

        // Assert
        _ = result.ShouldBeOfType<bool>();
        result.ShouldBeTrue();
    }

    [Theory]
    [InlineData(-100)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(10)]
    public void Delete_ShouldReturn_ActionFailedException(int id)
    {
        // Arrange
        var dataSource = Substitute.For<HardCodedNotesDataSource>();
        var context = new HardCodedNotesContext(dataSource);
        var token = new CancellationTokenSource().Token;

        _ = dataSource.NewBool().Returns(false);

        // Act
        var exception = Should.Throw<ActionFailedException>(async () => await context.Delete(id, token));

        // Assert
        _ = exception.ShouldNotBeNull();
        _ = exception.ShouldBeOfType<ActionFailedException>();
        exception.Entity.ShouldBe(EntityType.Notes);
        exception.Action.ShouldBe(ActionType.Delete);
        exception.Message.ShouldBe("Note failed to Delete from the Data Source");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Delete_ShouldReturn_NotFoundException(int id)
    {
        // Arrange
        var dataSource = Substitute.For<HardCodedNotesDataSource>();
        var context = new HardCodedNotesContext(dataSource);
        var token = new CancellationTokenSource().Token;

        // Act
        var exception = Should.Throw<NotFoundException>(async () => await context.Delete(id, token));

        // Assert
        _ = exception.ShouldNotBeNull();
        _ = exception.ShouldBeOfType<NotFoundException>();
        exception.Entity.ShouldBe(EntityType.Notes);
        exception.Message.ShouldBe("Note was not found in the Data Source");
    }

    #endregion Delete

    #region List

    [Fact]
    public async void List_ShouldReturnNotes()
    {
        // Arrange
        var dataSource = Substitute.For<HardCodedNotesDataSource>();
        var context = new HardCodedNotesContext(dataSource);
        var token = new CancellationTokenSource().Token;
        var id = 42;
        var text = "Meaning of Life";

        _ = dataSource.NewNote(text).ReturnsForAnyArgs(new Note(id, text));

        // Act
        var result = await context.List(token);

        // Assert
        _ = result.ShouldNotBeNull();
        _ = result.ShouldBeOfType<List<Note>>();
        result.Count.ShouldBe(3);

        _ = result[0].ShouldNotBeNull();
        result[0].Id.ShouldBe(id);
        result[0].Text.ShouldBe(text);

        _ = result[1].ShouldNotBeNull();
        result[1].Id.ShouldBe(id);
        result[1].Text.ShouldBe(text);

        _ = result[2].ShouldNotBeNull();
        result[2].Id.ShouldBe(id);
        result[2].Text.ShouldBe(text);
    }

    #endregion List

    #region Lookup

    [Fact]
    public async void Lookup_ShouldReturn_Note()
    {
        // Arrange
        var dataSource = Substitute.For<HardCodedNotesDataSource>();
        var context = new HardCodedNotesContext(dataSource);
        var token = new CancellationTokenSource().Token;
        var id = 42;
        var text = "Meaning of Life";

        _ = dataSource.NewNote(id, text).ReturnsForAnyArgs(new Note(id, text));

        // Act
        var result = await context.Lookup(id, token);

        // Assert
        _ = result.ShouldNotBeNull();
        result.Id.ShouldBe(id);
        result.Text.ShouldBe(text);
    }

    [Theory]
    [InlineData(-100)]
    [InlineData(-1)]
    [InlineData(0)]
    public void Lookup_ShouldReturn_ActionFailedException(int id)
    {
        // Arrange
        var dataSource = Substitute.For<HardCodedNotesDataSource>();
        var context = new HardCodedNotesContext(dataSource);
        var token = new CancellationTokenSource().Token;

        // Act
        var exception = Should.Throw<ActionFailedException>(async () => await context.Lookup(id, token));

        // Assert
        _ = exception.ShouldNotBeNull();
        _ = exception.ShouldBeOfType<ActionFailedException>();
        exception.Entity.ShouldBe(EntityType.Notes);
        exception.Action.ShouldBe(ActionType.Lookup);
        exception.Message.ShouldBe("Note failed the Lookup in the Data Source");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Lookup_ShouldReturn_NotFoundException(int id)
    {
        // Arrange
        var dataSource = Substitute.For<HardCodedNotesDataSource>();
        var context = new HardCodedNotesContext(dataSource);
        var token = new CancellationTokenSource().Token;

        // Act
        var exception = Should.Throw<NotFoundException>(async () => await context.Lookup(id, token));

        // Assert
        _ = exception.ShouldNotBeNull();
        _ = exception.ShouldBeOfType<NotFoundException>();
        exception.Entity.ShouldBe(EntityType.Notes);
        exception.Message.ShouldBe("Note was not found in the Data Source");
    }

    #endregion Lookup

    #region Update

    [Fact]
    public async void Update_ShouldReturnNote()
    {
        // Arrange
        var dataSource = Substitute.For<HardCodedNotesDataSource>();
        var context = new HardCodedNotesContext(dataSource);
        var token = new CancellationTokenSource().Token;
        var id = 42;
        var text = "Meaning of Life";

        _ = dataSource.NewNote(id, text).ReturnsForAnyArgs(new Note(id, text));

        // Act
        var result = await context.Update(id, text, token);

        // Assert
        _ = result.ShouldNotBeNull();
        result.Id.ShouldBe(id);
        result.Text.ShouldBe(text);
    }

    [Theory]
    [InlineData(-100)]
    [InlineData(-1)]
    [InlineData(0)]
    public void Update_ShouldReturn_ActionFailedException(int id)
    {
        // Arrange
        var dataSource = Substitute.For<HardCodedNotesDataSource>();
        var context = new HardCodedNotesContext(dataSource);
        var token = new CancellationTokenSource().Token;
        var text = "Meaning of Life";

        // Act
        var exception = Should.Throw<ActionFailedException>(async () => await context.Update(id, text, token));

        // Assert
        _ = exception.ShouldNotBeNull();
        _ = exception.ShouldBeOfType<ActionFailedException>();
        exception.Entity.ShouldBe(EntityType.Notes);
        exception.Action.ShouldBe(ActionType.Update);
        exception.Message.ShouldBe("Note failed to Update in the Data Source");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Update_ShouldReturn_NotFoundException(int id)
    {
        // Arrange
        var dataSource = Substitute.For<HardCodedNotesDataSource>();
        var context = new HardCodedNotesContext(dataSource);
        var token = new CancellationTokenSource().Token;
        var text = "Meaning of Life";

        // Act
        var exception = Should.Throw<NotFoundException>(async () => await context.Update(id, text, token));

        // Assert
        _ = exception.ShouldNotBeNull();
        _ = exception.ShouldBeOfType<NotFoundException>();
        exception.Entity.ShouldBe(EntityType.Notes);
        exception.Message.ShouldBe("Note was not found in the Data Source");
    }

    #endregion Update
}
