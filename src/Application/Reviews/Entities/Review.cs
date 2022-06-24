namespace CleanMinimalApi.Application.Reviews.Entities;

using System.ComponentModel.DataAnnotations.Schema;
using CleanMinimalApi.Application.Authors.Entities;
using CleanMinimalApi.Application.Common.Entities;
using CleanMinimalApi.Application.Movies.Entities;

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
