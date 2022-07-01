namespace CleanMinimalApi.Application.Reviews.Entities;

using Application.Authors.Entities;
using Application.Common.Entities;
using Application.Movies.Entities;

public record Review : Entity
{
    public int Stars { get; init; }

    public Movie ReviewedMovie { get; init; }

    public Author ReviewAuthor { get; init; }
}
