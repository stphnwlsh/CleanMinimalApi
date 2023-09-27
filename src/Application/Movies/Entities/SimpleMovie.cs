namespace CleanMinimalApi.Application.Movies.Entities;

using Application.Common.Entities;

public record SimpleMovie : Entity
{
    public string Title { get; init; }
}
