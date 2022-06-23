namespace CleanMinimalApi.Application.Reviews.Update;

using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Reviews;
using Common.Enums;
using Common.Exceptions;
using MediatR;

public class UpdateHandler : IRequestHandler<UpdateCommand, bool>
{
    private readonly IReviewsRepository repository;

    public UpdateHandler(IReviewsRepository repository) => this.repository = repository;

    public async Task<bool> Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        if (!await this.repository.ReviewExists(request.Id, cancellationToken))
        {
            NotFoundException.Throw(EntityType.Review);
        }

        return await this.repository.UpdateReview(request.Id, request.AuthorId, request.MovieId, request.Stars, cancellationToken);
    }
}
