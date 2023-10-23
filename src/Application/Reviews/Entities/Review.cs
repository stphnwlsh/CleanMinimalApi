namespace CleanMinimalApi.Application.Reviews.Entities;

using Application.Authors.Entities;
using Application.Common.Entities;
using Application.Movies.Entities;

public record Review : Entity
{
    public int Stars { get; init; }

    public ReviewMovie ReviewedMovie { get; init; }

    public ReviewAuthor ReviewAuthor { get; init; }
}
