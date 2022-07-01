namespace CleanMinimalApi.Application.Movies.Entities;

using Application.Common.Entities;
using Application.Reviews.Entities;

public record Movie : Entity
{
    public string Title { get; init; }

    public ICollection<Review> Reviews { get; init; }
}
