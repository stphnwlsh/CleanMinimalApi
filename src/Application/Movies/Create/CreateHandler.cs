namespace CleanMinimalApi.Application.Movies.Create;

using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Movies;
using CleanMinimalApi.Application.Reviews;
using Common.Enums;
using Common.Exceptions;
using Entities;
using MediatR;

public class CreateHandler : IRequestHandler<CreateCommand, Review>
{
    private readonly AuthorsRepository authorsRepository;
    private readonly IMoviesRepository moviesRepository;
    private readonly IReviewsRepository reviewsRepository;

    public CreateHandler(AuthorsRepository authorsRepository, IMoviesRepository moviesRepository, IReviewsRepository reviewsRepository)
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
