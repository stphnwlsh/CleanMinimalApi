namespace CleanMinimalApi.Application.Entities;

public class Author : Entity
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public ICollection<Review> Reviews { get; init; }
}
