namespace CleanMinimalApi.Application.Reviews.Commands.UpdateReview;

using System.Threading;
using System.Threading.Tasks;
using Common.Enums;
using Common.Exceptions;
using MediatR;

public class UpdateReviewHandler : IRequestHandler<UpdateReviewCommand, bool>
{
    private readonly IReviewsRepository repository;

    public UpdateReviewHandler(IReviewsRepository repository)
    {
        this.repository = repository;
    }

    public async Task<bool> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
    {
        if (!await this.repository.ReviewExists(request.Id, cancellationToken))
        {
            NotFoundException.Throw(EntityType.Review);
        }

        return await this.repository.UpdateReview(request.Id, request.AuthorId, request.MovieId, request.Stars, cancellationToken);
    }
}
