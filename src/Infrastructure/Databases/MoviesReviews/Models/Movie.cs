namespace CleanMinimalApi.Infrastructure.Databases.MoviesReviews.Models;

using System.ComponentModel.DataAnnotations.Schema;

internal record Movie : Entity
{
    public string Title { get; init; }

    [InverseProperty("ReviewedMovie")]
    public ICollection<Review> Reviews { get; init; }
}
