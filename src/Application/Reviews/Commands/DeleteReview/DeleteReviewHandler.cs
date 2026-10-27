namespace CleanMinimalApi.Application.Reviews.Commands.DeleteReview;

using System.Threading;
using System.Threading.Tasks;
using Common.Enums;
using Common.Exceptions;
using Mediator;

public class DeleteReviewHandler(IReviewsRepository repository) : IRequestHandler<DeleteReviewCommand, bool>
{
    public async ValueTask<bool> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {
        if (!await repository.ReviewExists(request.Id, cancellationToken))
        {
            NotFoundException.Throw(EntityType.Review);
        }

        return await repository.DeleteReview(request.Id, cancellationToken);
    }
}
