namespace CleanMinimalApi.Application.Reviews.Queries.GetReviews;

using CleanMinimalApi.Application.Entities;
using MediatR;

public class GetReviewsQuery : IRequest<List<Review>>
{
}
