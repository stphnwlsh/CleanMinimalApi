namespace CleanMinimalApi.Application.Movies.Queries.GetMovieById;

using System.Threading;
using System.Threading.Tasks;
using Common.Enums;
using Common.Exceptions;
using Entities;
using MediatR;

public class GetMovieByIdHandler(IMoviesRepository repository) : IRequestHandler<GetMovieByIdQuery, Movie>
{
    public async Task<Movie> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetMovieById(request.Id, cancellationToken);

        NotFoundException.ThrowIfNull(result, EntityType.Movie);

        return result;
    }
}
