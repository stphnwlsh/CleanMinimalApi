namespace CleanMinimalApi.Application.Reviews.Create;

using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Common.Enums;
using CleanMinimalApi.Application.Common.Exceptions;
using CleanMinimalApi.Application.Common.Interfaces;
using CleanMinimalApi.Domain.Reviews.Entities;
using MediatR;

public class CreateHandler : IRequestHandler<CreateCommand, Review>
{
    private readonly IAuthorsRepository authorsRepository;
    private readonly IMoviesRepository moviesRepository;
    private readonly IReviewsRepository reviewsRepository;

    public CreateHandler(IAuthorsRepository authorsRepository, IMoviesRepository moviesRepository, IReviewsRepository reviewsRepository)
    {
        this.authorsRepository = authorsRepository;
        this.moviesRepository = moviesRepository;
        this.reviewsRepository = reviewsRepository;
    }

    public async Task<Review> Handle(CreateCommand request, CancellationToken cancellationToken)
    {
        if (!await this.authorsRepository.AuthorExists(request.AuthorId, cancellationToken))
        {
            NotFoundException.Throw(EntityType.Author);
        }

        if (!await this.moviesRepository.MovieExists(request.MovieId, cancellationToken))
        {
            NotFoundException.Throw(EntityType.Movie);
        }

        return await this.reviewsRepository.CreateReview(request.AuthorId, request.MovieId, request.Stars, cancellationToken);
    }
}
