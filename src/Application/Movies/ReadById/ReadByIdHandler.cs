namespace CleanMinimalApi.Application.Movies.ReadById;

using System.Threading;
using System.Threading.Tasks;
using Common.Enums;
using Common.Exceptions;
using Common.Interfaces;
using Entities;
using MediatR;

public class ReadByIdHandler : IRequestHandler<ReadByIdQuery, Movie>
{
    private readonly MoviesRepository repository;

    public ReadByIdHandler(MoviesRepository repository)
    {
        this.repository = repository;
    }

    public async Task<Movie> Handle(ReadByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await this.repository.ReadMovieById(request.Id, cancellationToken);

        NotFoundException.ThrowIfNull(result, EntityType.Movie);

        return result;
    }
}
