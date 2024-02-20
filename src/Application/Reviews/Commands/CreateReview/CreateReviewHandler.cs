namespace CleanMinimalApi.Application.Reviews.Commands.CreateReview;

using System.Threading;
using System.Threading.Tasks;
using Authors;
using Common.Enums;
using Common.Exceptions;
using Entities;
using MediatR;
using Movies;

public class CreateReviewHandler(
    IAuthorsRepository authorsRepository,
    IMoviesRepository moviesRepository,
    IReviewsRepository reviewsRepository) : IRequestHandler<CreateReviewCommand, Review>
{
    public async Task<Review> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        if (!await authorsRepository.AuthorExists(request.AuthorId, cancellationToken))
        {
            NotFoundException.Throw(EntityType.Author);
        }

        if (!await moviesRepository.MovieExists(request.MovieId, cancellationToken))
        {
            NotFoundException.Throw(EntityType.Movie);
        }

        return await reviewsRepository
            .CreateReview(request.AuthorId, request.MovieId, request.Stars, cancellationToken);
    }
}
