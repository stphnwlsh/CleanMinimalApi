namespace CleanMinimalApi.Infrastructure.Tests.Integration.Databases.MovieReviews;

using Infrastructure.Databases.MovieReviews;
using Shouldly;
using Xunit;

public class MovieReviewsDatabaseTests
{
    [Theory]
    [InlineData("Data Source=movie-reviews.db", false)]
    [InlineData("Data Source=Movies;Mode=Memory;Cache=Shared", true)]
    public void IsInMemory_ShouldMatch_TheConnectionString(string connectionString, bool expected)
    {
        // Arrange
        using var database = new MovieReviewsDatabase(connectionString);

        // Act
        var result = database.IsInMemory;

        // Assert
        result.ShouldBe(expected);
    }

    [Fact]
    public void KeepAlive_ShouldOnlyOpen_OneConnection()
    {
        // Arrange
        using var database = new MovieReviewsDatabase($"Data Source=Tests-{Guid.NewGuid()};Mode=Memory;Cache=Shared");

        // Act
        database.KeepAlive();
        database.KeepAlive();

        // Assert
        database.IsInMemory.ShouldBeTrue();
    }

    [Fact]
    public void KeepAlive_ShouldDoNothing_ForAFile()
    {
        // Arrange
        using var database = new MovieReviewsDatabase("Data Source=never-created.db");

        // Act
        database.KeepAlive();

        // Assert
        File.Exists("never-created.db").ShouldBeFalse();
    }
}
