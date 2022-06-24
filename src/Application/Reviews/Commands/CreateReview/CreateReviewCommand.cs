namespace CleanMinimalApi.Application.Reviews.Commands.CreateReview;

using CleanMinimalApi.Application.Entities;
using MediatR;

public class CreateReviewCommand : IRequest<Review>
{
    public Guid AuthorId { get; set; }
    public Guid MovieId { get; set; }
    public int Stars { get; set; }
}
