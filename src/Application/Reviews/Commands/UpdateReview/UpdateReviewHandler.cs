namespace CleanMinimalApi.Application.Reviews.Commands.UpdateReview;

using Common.Enums;
using Common.Exceptions;
using Mediator;

public class UpdateReviewHandler(IReviewsRepository reviewsRepository) : IRequestHandler<UpdateReviewCommand, bool>
{
    public async ValueTask<bool> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
    {
        if (!await reviewsRepository.ReviewExists(request.Id, cancellationToken))
        {
            NotFoundException.Throw(EntityType.Review);
        }

        return await reviewsRepository.UpdateReview(request.Id, request.Stars, cancellationToken);
    }
}
