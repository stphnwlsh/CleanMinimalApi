namespace CleanMinimalApi.Application.Reviews.Commands.UpdateReview;

using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Authors;
using CleanMinimalApi.Application.Movies;
using Common.Enums;
using Common.Exceptions;
using MediatR;

public class UpdateReviewHandler(
    IAuthorsRepository authorsRepository,
    IMoviesRepository moviesRepository,
    IReviewsRepository reviewsRepository) : IRequestHandler<UpdateReviewCommand, bool>
{
    public async Task<bool> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
    {
        if (!await reviewsRepository.ReviewExists(request.Id, cancellationToken))
        {
            NotFoundException.Throw(EntityType.Review);
        }

        if (!await authorsRepository.AuthorExists(request.AuthorId, cancellationToken))
        {
            NotFoundException.Throw(EntityType.Author);
        }

        if (!await moviesRepository.MovieExists(request.MovieId, cancellationToken))
        {
            NotFoundException.Throw(EntityType.Movie);
        }

        return await reviewsRepository
            .UpdateReview(request.Id, request.AuthorId, request.MovieId, request.Stars, cancellationToken);
    }
}
