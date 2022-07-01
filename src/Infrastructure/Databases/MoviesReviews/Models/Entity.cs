namespace CleanMinimalApi.Infrastructure.Databases.MoviesReviews.Models;

internal abstract record Entity
{
    public Guid Id { get; init; }
    public DateTime DateCreated { get; init; }
    public DateTime DateModified { get; set; }
}
