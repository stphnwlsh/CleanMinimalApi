namespace CleanMinimalApi.Infrastructure.Databases.MoviesReviews.Models;

public abstract class Entity
{
    public Guid Id { get; init; }
    public DateTime DateCreated { get; init; }
    public DateTime DateModified { get; set; }
}
