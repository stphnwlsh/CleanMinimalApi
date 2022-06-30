namespace CleanMinimalApi.Application.Reviews.Entities;

using CleanMinimalApi.Application.Authors.Entities;
using CleanMinimalApi.Application.Common.Entities;
using CleanMinimalApi.Application.Movies.Entities;

public class Review : Entity
{
    public int Stars { get; init; }

    public Movie ReviewedMovie { get; set; }

    public Author ReviewAuthor { get; set; }
}
