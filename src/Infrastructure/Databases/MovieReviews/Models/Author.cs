namespace CleanMinimalApi.Infrastructure.Databases.MovieReviews.Models;

using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
internal record Author : Entity
{
    public required string FirstName { get; init; }

    public required string LastName { get; init; }

    public ICollection<Review> Reviews { get; init; } = [];
}
