namespace CleanMinimalApi.Application.Authors.ReadById;

using System.Threading;
using System.Threading.Tasks;
using Common.Enums;
using Common.Exceptions;
using Common.Interfaces;
using Entities;
using MediatR;

public class ReadByIdHandler : IRequestHandler<ReadByIdQuery, Author>
{
    private readonly AuthorsRepository repository;

    public ReadByIdHandler(AuthorsRepository repository)
    {
        this.repository = repository;
    }

    public async Task<Author> Handle(ReadByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await this.repository.ReadAuthorById(request.Id, cancellationToken);

        NotFoundException.ThrowIfNull(result, EntityType.Author);

        return result;
    }
}
