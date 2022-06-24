namespace CleanMinimalApi.Application.Reviews.Commands.CreateReview;

using System.Threading;
using System.Threading.Tasks;
using Authors;
using Common.Enums;
using Common.Exceptions;
using Entities;
using MediatR;
using Movies;

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
