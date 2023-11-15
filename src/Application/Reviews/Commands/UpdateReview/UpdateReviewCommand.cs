namespace CleanMinimalApi.Application.Reviews.Commands.UpdateReview;

using MediatR;

public class UpdateReviewCommand : IRequest<bool>
{
    public Guid Id { get; set; }

    public Guid AuthorId { get; init; }

    public Guid MovieId { get; init; }

    public int Stars { get; init; }
}
