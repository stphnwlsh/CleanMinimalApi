namespace CleanMinimalApi.Application.Reviews.Queries.GetReviewById;

using Entities;
using MediatR;

public class GetReviewByIdQuery : IRequest<Review>
{
    public Guid Id { get; init; }
}
