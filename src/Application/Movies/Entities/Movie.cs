namespace CleanMinimalApi.Application.Movies.Entities;

using CleanMinimalApi.Application.Common.Entities;
using CleanMinimalApi.Application.Reviews.Entities;

public record Movie : Entity
{
    public string Title { get; init; }

    public ICollection<Review> Reviews { get; init; }
}
