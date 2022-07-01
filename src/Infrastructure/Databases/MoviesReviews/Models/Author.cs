namespace CleanMinimalApi.Infrastructure.Databases.MoviesReviews.Models;

using System.ComponentModel.DataAnnotations.Schema;

internal record Author : Entity
{
    public string FirstName { get; init; }

    public string LastName { get; init; }

    [InverseProperty("ReviewAuthor")]
    public ICollection<Review> Reviews { get; init; }
}
