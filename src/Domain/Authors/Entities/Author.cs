namespace CleanMinimalApi.Domain.Authors.Entities;

using System.ComponentModel.DataAnnotations.Schema;
using CleanMinimalApi.Domain.Common.Entity;
using CleanMinimalApi.Domain.Reviews.Entities;

public class Author : Entity
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    [InverseProperty("ReviewAuthor")]
    public ICollection<Review> Reviews { get; init; }
}
