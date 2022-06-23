namespace CleanMinimalApi.Application.Reviews.ReadAll;

using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using Entities;
using MediatR;

public class ReadAllHandler : IRequestHandler<ReadAllQuery, List<Review>>
{
    private readonly ReviewsRepository repository;

    public ReadAllHandler(ReviewsRepository repository)
    {
        this.repository = repository;
    }

    public async Task<List<Review>> Handle(ReadAllQuery request, CancellationToken cancellationToken)
    {
        return await this.repository.ReadAllReviews(cancellationToken);
    }
}
