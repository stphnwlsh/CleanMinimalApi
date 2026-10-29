namespace CleanMinimalApi.Application.Authors.Entities;

/// <summary>
/// An author in a list, with how many reviews they have written and their average star rating instead of the reviews themselves.
/// </summary>
/// <param name="Id">The unique identifier of the author.</param>
/// <param name="FirstName">The author's first name.</param>
/// <param name="LastName">The author's last name.</param>
/// <param name="ReviewCount">The number of reviews the author has written.</param>
/// <param name="AverageStars">The average star rating the author gives to one decimal place, or null when they have no reviews.</param>
public record AuthorSummary(Guid Id, string FirstName, string LastName, int ReviewCount, double? AverageStars);
