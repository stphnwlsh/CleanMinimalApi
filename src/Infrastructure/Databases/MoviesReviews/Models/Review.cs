namespace CleanMinimalApi.Infrastructure.Databases.MoviesReviews.Models;

using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
internal record Review : Entity
{
    public int Stars { get; set; }

    [ForeignKey("ReviewedMovie")]
    public Guid ReviewedMovieId { get; set; }

    public Movie ReviewedMovie { get; init; }

    [ForeignKey("ReviewAuthor")]
    public Guid ReviewAuthorId { get; set; }

    public Author ReviewAuthor { get; init; }
}
