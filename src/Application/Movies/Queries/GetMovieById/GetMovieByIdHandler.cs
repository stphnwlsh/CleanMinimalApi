namespace CleanMinimalApi.Application.Movies.Queries.GetMovieById;

using System.Threading;
using System.Threading.Tasks;
using Common.Enums;
using Common.Exceptions;
using Entities;
using MediatR;

public class GetMovieByIdHandler : IRequestHandler<GetMovieByIdQuery, Movie>
{
    private readonly IMoviesRepository repository;

    public GetMovieByIdHandler(IMoviesRepository repository)
    {
        this.repository = repository;
    }

    public async Task<Movie> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await this.repository.GetMovieById(request.Id, cancellationToken);

        NotFoundException.ThrowIfNull(result, EntityType.Movie);

        return result;
    }
}
