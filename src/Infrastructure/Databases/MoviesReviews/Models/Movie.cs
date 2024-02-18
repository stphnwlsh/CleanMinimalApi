namespace CleanMinimalApi.Infrastructure.Databases.MoviesReviews.Models;
internal record Movie : Entity
{
    public string Title { get; init; }

    public ICollection<Review> Reviews { get; init; }
}
