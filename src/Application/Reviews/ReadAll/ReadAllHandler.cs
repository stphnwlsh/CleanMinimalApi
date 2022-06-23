namespace CleanMinimalApi.Application.Reviews.ReadAll;

using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Reviews;
using Entities;
using MediatR;

public class ReadAllHandler : IRequestHandler<ReadAllQuery, List<Review>>
{
    private readonly IReviewsRepository repository;

    public ReadAllHandler(IReviewsRepository repository) => this.repository = repository;

    public async Task<List<Review>> Handle(ReadAllQuery request, CancellationToken cancellationToken) => await this.repository.ReadAllReviews(cancellationToken);
}
