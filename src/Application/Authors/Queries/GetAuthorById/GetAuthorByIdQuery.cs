namespace CleanMinimalApi.Application.Authors.Queries.GetAuthorById;

using Entities;
using MediatR;

public class GetAuthorByIdQuery : IRequest<Author>
{
    public Guid Id { get; init; }
}
