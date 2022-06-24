namespace CleanMinimalApi.Application.Authors.Entities;

using System.ComponentModel.DataAnnotations.Schema;
using CleanMinimalApi.Application.Common.Entities;
using CleanMinimalApi.Application.Reviews.Entities;

public class Author : Entity
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    [InverseProperty("ReviewAuthor")]
    public ICollection<Review> Reviews { get; init; }
}
