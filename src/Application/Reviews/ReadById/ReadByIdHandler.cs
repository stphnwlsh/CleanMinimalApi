namespace CleanMinimalApi.Application.Reviews.ReadById;

using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Reviews;
using Common.Enums;
using Common.Exceptions;
using Entities;
using MediatR;

public class ReadByIdHandler : IRequestHandler<ReadByIdQuery, Review>
{
    private readonly IReviewsRepository repository;

    public ReadByIdHandler(IReviewsRepository repository) => this.repository = repository;

    public async Task<Review> Handle(ReadByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await this.repository.ReadReviewById(request.Id, cancellationToken);

        NotFoundException.ThrowIfNull(result, EntityType.Review);

        return result;
    }
}
