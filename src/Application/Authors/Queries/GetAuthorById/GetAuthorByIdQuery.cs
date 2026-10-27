namespace CleanMinimalApi.Application.Authors.Queries.GetAuthorById;

using System.ComponentModel.DataAnnotations;
using Entities;
using Mediator;

public class GetAuthorByIdQuery : IRequest<Author>
{
    [Required]
    public Guid Id { get; init; }
}
