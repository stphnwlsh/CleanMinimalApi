namespace CleanMinimalApi.Application.Reviews.Queries.GetReviewById;

using CleanMinimalApi.Application.Entities;
using MediatR;

public class GetReviewByIdQuery : IRequest<Review>
{
    public Guid Id { get; set; }
}
