namespace CleanMinimalApi.Infrastructure.Tests.Integration.Persistance.HardCoded;

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using CleanMinimalApi.Infrastructure.Persistance.HardCoded;
using Shouldly;
using Xunit;

[ExcludeFromCodeCoverage]
public class HardCodedNotesContextTests
{
    #region Create

    [Fact]
    public async void Create_ShouldReturnNote()
    {
        // Arrange
        var context = new HardCodedNotesContext(new HardCodedNotesDataSource());
        var token = new CancellationTokenSource().Token;
        var text = "Meaning of Life";

        // Act
        var result = await context.Create(text, token);

        // Assert
        _ = result.ShouldNotBeNull();
        result.Id.ShouldBeInRange(0, 100);
        result.Text.ShouldBe(text);
    }

    #endregion Create

    #region Delete

    [Fact]
    public async void Delete_ShouldReturnBoolean()
    {
        // Arrange
        var context = new HardCodedNotesContext(new HardCodedNotesDataSource());
        var token = new CancellationTokenSource().Token;

        // Act
        var result = await context.Delete(10, token);

        // Assert
        result.ShouldBeTrue();
    }

    #endregion Delete

    #region List

    [Fact]
    public async void List_ShouldReturnNotes()
    {
        // Arrange
        var context = new HardCodedNotesContext(new HardCodedNotesDataSource());
        var token = new CancellationTokenSource().Token;

        // Act
        var result = await context.List(token);

        // Assert
        _ = result.ShouldNotBeNull();
        result.Count.ShouldBe(3);

        _ = result[0].ShouldNotBeNull();
        result[0].Id.ShouldBeInRange(0, 100);
        result[0].Text.ShouldStartWith("List All Notes - ");

        _ = result[1].ShouldNotBeNull();
        result[1].Id.ShouldBeInRange(0, 100);
        result[1].Text.ShouldStartWith("List All Notes - ");

        _ = result[2].ShouldNotBeNull();
        result[2].Id.ShouldBeInRange(0, 100);
        result[2].Text.ShouldStartWith("List All Notes - ");
    }

    #endregion List

    #region Lookup

    [Fact]
    public async void Lookup_ShouldReturnNote()
    {
        // Arrange
        var context = new HardCodedNotesContext(new HardCodedNotesDataSource());
        var token = new CancellationTokenSource().Token;
        var id = 42;

        // Act
        var result = await context.Lookup(id, token);

        // Assert
        _ = result.ShouldNotBeNull();
        result.Id.ShouldBe(id);
        result.Text.ShouldStartWith("Lookup Note - ");
    }

    #endregion Lookup

    #region Update

    [Fact]
    public async void Update_ShouldReturnNote()
    {
        // Arrange
        var context = new HardCodedNotesContext(new HardCodedNotesDataSource());
        var token = new CancellationTokenSource().Token;
        var id = 42;
        var text = "Meaning of Life";

        // Act
        var result = await context.Update(id, text, token);

        // Assert
        _ = result.ShouldNotBeNull();
        result.Id.ShouldBe(id);
        result.Text.ShouldBe(text);
    }

    #endregion Update
}
