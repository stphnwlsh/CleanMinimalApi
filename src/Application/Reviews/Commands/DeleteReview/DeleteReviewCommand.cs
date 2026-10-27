namespace CleanMinimalApi.Application.Reviews.Commands.DeleteReview;

using Mediator;

public class DeleteReviewCommand : IRequest<bool>
{
    public Guid Id { get; init; }
}
