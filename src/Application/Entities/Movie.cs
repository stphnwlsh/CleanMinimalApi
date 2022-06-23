namespace CleanMinimalApi.Application.Entities;

public class Movie : Entity
{
    public string Title { get; set; }

    public ICollection<Review> Reviews { get; init; }
}
