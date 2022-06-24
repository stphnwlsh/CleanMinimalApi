namespace CleanMinimalApi.Application.Authors.Queries.GetAuthors;

using CleanMinimalApi.Application.Entities;
using MediatR;

public class GetAuthorsQuery : IRequest<List<Author>>
{
}
