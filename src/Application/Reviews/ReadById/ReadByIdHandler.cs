namespace CleanMinimalApi.Application.Reviews.ReadById;

using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Common.Enums;
using CleanMinimalApi.Application.Common.Exceptions;
using CleanMinimalApi.Application.Common.Interfaces;
using CleanMinimalApi.Application.Entities;
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
