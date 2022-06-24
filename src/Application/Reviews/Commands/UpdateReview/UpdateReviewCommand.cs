namespace CleanMinimalApi.Application.Reviews.Commands.UpdateReview;

using MediatR;

public class UpdateReviewCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public Guid AuthorId { get; set; }
    public Guid MovieId { get; set; }
    public int Stars { get; set; }
}
