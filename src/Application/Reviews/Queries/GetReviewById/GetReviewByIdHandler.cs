namespace CleanMinimalApi.Application.Reviews.Queries.GetReviewById;

using System.Threading;
using System.Threading.Tasks;
using Common.Enums;
using Common.Exceptions;
using Entities;
using MediatR;

public class GetReviewByIdHandler(IReviewsRepository repository) : IRequestHandler<GetReviewByIdQuery, Review>
{
    public async Task<Review> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetReviewById(request.Id, cancellationToken);

        NotFoundException.ThrowIfNull(result, EntityType.Review);

        return result;
    }
}
