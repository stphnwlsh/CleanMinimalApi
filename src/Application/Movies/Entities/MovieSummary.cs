namespace CleanMinimalApi.Application.Movies.Entities;

/// <summary>
/// A movie in a list, with how many reviews it has and their average star rating instead of the reviews themselves.
/// </summary>
/// <param name="Id">The unique identifier of the movie.</param>
/// <param name="Title">The title of the movie.</param>
/// <param name="ReviewCount">The number of reviews written about the movie.</param>
/// <param name="AverageStars">The average star rating to one decimal place, or null when the movie has no reviews.</param>
public record MovieSummary(Guid Id, string Title, int ReviewCount, double? AverageStars);
