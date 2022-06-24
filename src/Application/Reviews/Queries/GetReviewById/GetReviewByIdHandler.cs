namespace CleanMinimalApi.Application.Reviews.Queries.GetReviewById;

using System.Threading;
using System.Threading.Tasks;
using Common.Enums;
using Common.Exceptions;
using Entities;
using MediatR;

public class GetReviewByIdHandler : IRequestHandler<GetReviewByIdQuery, Review>
{
    private readonly IReviewsRepository repository;

    public GetReviewByIdHandler(IReviewsRepository repository)
    {
        this.repository = repository;
    }

    public async Task<Review> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await this.repository.GetReviewById(request.Id, cancellationToken);

        NotFoundException.ThrowIfNull(result, EntityType.Review);

        return result;
    }
}
