namespace CleanMinimalApi.Domain.Authors.Entities;

using System.ComponentModel.DataAnnotations.Schema;
using CleanMinimalApi.Domain.Common.Entity;
using CleanMinimalApi.Domain.Reviews.Entities;

public class Author : Entity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    [InverseProperty("ReviewAuthor")]
    public ICollection<Review> Reviews { get; } = new List<Review>();
}
