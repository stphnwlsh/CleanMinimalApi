namespace CleanMinimalApi.Application.Authors.Queries.GetAuthors;

using Entities;
using Mediator;

public class GetAuthorsQuery : IRequest<List<Author>>
{
}
