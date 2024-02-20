namespace CleanMinimalApi.Infrastructure.Databases.MoviesReviews.Models;

using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
internal record Author : Entity
{
    public string FirstName { get; init; }

    public string LastName { get; init; }

    public ICollection<Review> Reviews { get; init; }
}
