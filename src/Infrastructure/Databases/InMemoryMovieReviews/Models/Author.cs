namespace CleanMinimalApi.Infrastructure.Databases.InMemoryMovieReviews.Models;

using System.ComponentModel.DataAnnotations.Schema;

public class Author : Entity
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    [InverseProperty("ReviewAuthor")]
    public ICollection<Review> Reviews { get; init; }
}
