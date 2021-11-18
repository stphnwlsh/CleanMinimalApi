namespace CleanMinimalApi.Domain.Movies.Entities;

using System.ComponentModel.DataAnnotations.Schema;
using CleanMinimalApi.Domain.Common.Entity;
using CleanMinimalApi.Domain.Reviews.Entities;

public class Movie : Entity
{
    public string Title { get; set; }

    [InverseProperty("ReviewedMovie")]
    public ICollection<Review> Reviews { get; init; }
}
