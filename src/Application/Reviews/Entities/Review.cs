namespace CleanMinimalApi.Application.Reviews.Entities;

using CleanMinimalApi.Application.Authors.Entities;
using CleanMinimalApi.Application.Common.Entities;
using CleanMinimalApi.Application.Movies.Entities;

public class Review : Entity
{
    public int Stars { get; set; }

    public Movie ReviewedMovie { get; init; }

    public Author ReviewAuthor { get; init; }
}
