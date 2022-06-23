namespace CleanMinimalApi.Application.Authors.Delete;

using System.Threading;
using System.Threading.Tasks;
using Common.Enums;
using Common.Exceptions;
using MediatR;

public class DeleteHandler : IRequestHandler<DeleteCommand, bool>
{
    private readonly IAuthorsRepository repository;

    public DeleteHandler(IAuthorsRepository repository) => this.repository = repository;

    public async Task<bool> Handle(DeleteCommand request, CancellationToken cancellationToken)
    {
        if (!await this.repository.AuthorExists(request.Id, cancellationToken))
        {
            NotFoundException.Throw(EntityType.Author);
        }

        if (await this.repository.AuthorHasReviews(request.Id, cancellationToken))
        {
            OrphanedReviewsException.Throw(EntityType.Author);
        }

        return await this.repository.DeleteAuthor(request.Id, cancellationToken);
    }
}
