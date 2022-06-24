namespace CleanMinimalApi.Application.Authors.Queries.GetAuthorById;

using CleanMinimalApi.Application.Entities;
using MediatR;

public class GetAuthorByIdQuery : IRequest<Author>
{
    public Guid Id { get; set; }
}
