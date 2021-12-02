namespace CleanMinimalApi.Application.Entities;

using System.ComponentModel.DataAnnotations.Schema;

public class Author : Entity
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    [InverseProperty("ReviewAuthor")]
    public ICollection<Review> Reviews { get; init; }
}
