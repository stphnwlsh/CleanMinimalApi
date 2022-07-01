namespace CleanMinimalApi.Application.Authors.Entities;

using CleanMinimalApi.Application.Common.Entities;
using CleanMinimalApi.Application.Reviews.Entities;

public record Author : Entity
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public ICollection<Review> Reviews { get; set; }
}
