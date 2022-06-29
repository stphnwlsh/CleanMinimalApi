namespace CleanMinimalApi.Infrastructure.Databases.MoviesReviews.Models;

using System.ComponentModel.DataAnnotations.Schema;

public class Movie : Entity
{
    public string Title { get; set; }

    [InverseProperty("ReviewedMovie")]
    public ICollection<Review> Reviews { get; init; }
}
