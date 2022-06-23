namespace CleanMinimalApi.Application.Entities;


public class Review : Entity
{
    public int Stars { get; set; }

    public Guid ReviewedMovieId { get; set; }

    public Movie ReviewedMovie { get; init; }

    public Guid ReviewAuthorId { get; set; }

    public Author ReviewAuthor { get; init; }
}
