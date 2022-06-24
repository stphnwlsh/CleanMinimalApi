namespace CleanMinimalApi.Application.Authors.Queries.GetAuthorById;

using System.Threading;
using System.Threading.Tasks;
using Common.Enums;
using Common.Exceptions;
using Entities;
using MediatR;

public class GetAuthorByIdHandler : IRequestHandler<GetAuthorByIdQuery, Author>
{
    private readonly IAuthorsRepository repository;

    public GetAuthorByIdHandler(IAuthorsRepository repository)
    {
        this.repository = repository;
    }

    public async Task<Author> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await this.repository.GetAuthorById(request.Id, cancellationToken);

        NotFoundException.ThrowIfNull(result, EntityType.Author);

        return result;
    }
}
