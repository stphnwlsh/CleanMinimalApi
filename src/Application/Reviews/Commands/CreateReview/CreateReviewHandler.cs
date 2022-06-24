namespace CleanMinimalApi.Application.Reviews.Commands.CreateReview;

using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Authors;
using CleanMinimalApi.Application.Common.Enums;
using CleanMinimalApi.Application.Common.Exceptions;
using CleanMinimalApi.Application.Entities;
using CleanMinimalApi.Application.Movies;
using MediatR;

public class CreateReviewHandler : IRequestHandler<CreateReviewCommand, Review>
{
    private readonly IAuthorsRepository authorsRepository;
    private readonly IMoviesRepository moviesRepository;
    private readonly IReviewsRepository reviewsRepository;

    public CreateReviewHandler(IAuthorsRepository authorsRepository, IMoviesRepository moviesRepository, IReviewsRepository reviewsRepository)
    {
        this.authorsRepository = authorsRepository;
        this.moviesRepository = moviesRepository;
        this.reviewsRepository = reviewsRepository;
    }

    public async Task<Review> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
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
