namespace CleanMinimalApi.Application.Reviews.Queries.GetReviewById;

using Entities;
using Mediator;

public class GetReviewByIdQuery : IRequest<Review>
{
    public Guid Id { get; init; }
}
