namespace CleanMinimalApi.Application.Authors.Entities;

using Application.Reviews.Entities;

/// <summary>
/// A person who writes movie reviews.
/// </summary>
/// <param name="Id">The unique identifier of the author.</param>
/// <param name="FirstName">The author's first name.</param>
/// <param name="LastName">The author's last name.</param>
/// <param name="Reviews">The reviews the author has written.</param>
public record Author(Guid Id, string FirstName, string LastName, ICollection<Review>? Reviews = null);
