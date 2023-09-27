namespace CleanMinimalApi.Application.Authors.Entities;

using Application.Common.Entities;

public record SimpleAuthor : Entity
{
    public string FirstName { get; set; }

    public string LastName { get; set; }
}
