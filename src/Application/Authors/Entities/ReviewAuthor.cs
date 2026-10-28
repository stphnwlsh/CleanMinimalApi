namespace CleanMinimalApi.Application.Authors.Entities;

/// <summary>
/// The author who wrote a review.
/// </summary>
/// <param name="Id">The unique identifier of the author.</param>
/// <param name="FirstName">The author's first name.</param>
/// <param name="LastName">The author's last name.</param>
public record ReviewAuthor(Guid Id, string FirstName, string LastName);
