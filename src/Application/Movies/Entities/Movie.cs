namespace CleanMinimalApi.Application.Movies.Entities;

using CleanMinimalApi.Application.Common.Entities;
using CleanMinimalApi.Application.Reviews.Entities;

public class Movie : Entity
{
    public string Title { get; set; }

    public ICollection<Review> Reviews { get; set; }
}
