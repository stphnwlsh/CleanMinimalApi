namespace CleanMinimalApi.Application.Movies.GetMovieById;

using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Common.Enums;
using CleanMinimalApi.Application.Common.Exceptions;
using CleanMinimalApi.Application.Entities;
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
