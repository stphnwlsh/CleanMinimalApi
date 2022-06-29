namespace CleanMinimalApi.Infrastructure.Databases.MoviesReviews.Models;

public abstract class Entity
{
    public Guid Id { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateModified { get; set; }
}
