namespace CleanMinimalApi.Application.Authors.Update;

using System.Threading;
using System.Threading.Tasks;
using Common.Enums;
using Common.Exceptions;
using Common.Interfaces;
using Entities;
using MediatR;

public class UpdateHandler : IRequestHandler<UpdateCommand, bool>
{
    private readonly AuthorsRepository repository;

    public UpdateHandler(AuthorsRepository repository)
    {
        this.repository = repository;
    }

    public async Task<bool> Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        if (!await this.repository.AuthorExists(request.Id, cancellationToken))
        {
            NotFoundException.Throw(EntityType.Review);
        }

        return await this.repository.UpdateAuthor(request.Id, request.FirstName, request.LastName, cancellationToken);
    }
}
