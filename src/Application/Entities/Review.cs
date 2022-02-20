namespace CleanMinimalApi.Application.Entities;

using System.ComponentModel.DataAnnotations.Schema;

public class Review : Entity
{
    public int Stars { get; set; }

    [ForeignKey("ReviewedMovie")]
    public Guid ReviewedMovieId { get; set; }

    public Movie ReviewedMovie { get; init; }

    [ForeignKey("ReviewAuthor")]
    public Guid ReviewAuthorId { get; set; }

    public Author ReviewAuthor { get; init; }
}
