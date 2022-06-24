namespace CleanMinimalApi.Application.Movies.Entities;

using System.ComponentModel.DataAnnotations.Schema;
using CleanMinimalApi.Application.Common.Entities;
using CleanMinimalApi.Application.Reviews.Entities;

public class Movie : Entity
{
    public string Title { get; set; }

    [InverseProperty("ReviewedMovie")]
    public ICollection<Review> Reviews { get; init; }
}
