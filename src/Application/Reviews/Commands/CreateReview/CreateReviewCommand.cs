namespace CleanMinimalApi.Application.Reviews.Commands.CreateReview;

using Entities;
using MediatR;

public class CreateReviewCommand : IRequest<Review>
{
    public Guid AuthorId { get; init; }

    public Guid MovieId { get; init; }

    public int Stars { get; init; }
}
