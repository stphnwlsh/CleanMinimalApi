namespace CleanMinimalApi.Application.Authors.Queries.GetAuthorById;

using System.Threading;
using System.Threading.Tasks;
using Common.Enums;
using Common.Exceptions;
using Entities;
using MediatR;

public class GetAuthorByIdHandler(IAuthorsRepository repository) : IRequestHandler<GetAuthorByIdQuery, Author>
{
    public async Task<Author> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetAuthorById(request.Id, cancellationToken);

        NotFoundException.ThrowIfNull(result, EntityType.Author);

        return result;
    }
}
