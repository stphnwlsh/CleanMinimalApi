namespace CleanMinimalApi.Infrastructure.Databases.MoviesReviews.Models;
internal record Author : Entity
{
    public string FirstName { get; init; }

    public string LastName { get; init; }

    public ICollection<Review> Reviews { get; init; }
}
