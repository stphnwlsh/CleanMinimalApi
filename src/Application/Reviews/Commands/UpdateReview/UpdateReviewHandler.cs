namespace CleanMinimalApi.Application.Reviews.Commands.UpdateReview;

using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Authors;
using CleanMinimalApi.Application.Movies;
using Common.Enums;
using Common.Exceptions;
using MediatR;

public class UpdateReviewHandler : IRequestHandler<UpdateReviewCommand, bool>
{
    private readonly IAuthorsRepository authorsRepository;
    private readonly IMoviesRepository moviesRepository;
    private readonly IReviewsRepository reviewsRepository;

    public UpdateReviewHandler(IAuthorsRepository authorsRepository, IMoviesRepository moviesRepository, IReviewsRepository reviewsRepository)
    {
        this.authorsRepository = authorsRepository;
        this.moviesRepository = moviesRepository;
        this.reviewsRepository = reviewsRepository;
    }

    public async Task<bool> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
    {
        if (!await this.reviewsRepository.ReviewExists(request.Id, cancellationToken))
        {
            NotFoundException.Throw(EntityType.Review);
        }

        if (!await this.authorsRepository.AuthorExists(request.AuthorId, cancellationToken))
        {
            NotFoundException.Throw(EntityType.Author);
        }

        if (!await this.moviesRepository.MovieExists(request.MovieId, cancellationToken))
        {
            NotFoundException.Throw(EntityType.Movie);
        }

        return await this.reviewsRepository.UpdateReview(request.Id, request.AuthorId, request.MovieId, request.Stars, cancellationToken);
    }
}
