namespace CleanMinimalApi.Application.Reviews.ReadById;

using System.Threading;
using System.Threading.Tasks;
using Common.Enums;
using Common.Exceptions;
using Common.Interfaces;
using Entities;
using MediatR;

public class ReadByIdHandler : IRequestHandler<ReadByIdQuery, Review>
{
    private readonly ReviewsRepository repository;

    public ReadByIdHandler(ReviewsRepository repository)
    {
        this.repository = repository;
    }

    public async Task<Review> Handle(ReadByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await this.repository.ReadReviewById(request.Id, cancellationToken);

        NotFoundException.ThrowIfNull(result, EntityType.Review);

        return result;
    }
}
