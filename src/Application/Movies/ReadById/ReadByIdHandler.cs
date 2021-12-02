namespace CleanMinimalApi.Application.Movies.ReadById;

using System.Threading;
using System.Threading.Tasks;
using CleanMinimalApi.Application.Common.Enums;
using CleanMinimalApi.Application.Common.Exceptions;
using CleanMinimalApi.Application.Common.Interfaces;
using CleanMinimalApi.Application.Entities;
using MediatR;

public class ReadByIdHandler : IRequestHandler<ReadByIdQuery, Movie?>
{
    private readonly IMoviesRepository repository;

    public ReadByIdHandler(IMoviesRepository repository)
    {
        this.repository = repository;
    }

    public async Task<Movie?> Handle(ReadByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await this.repository.ReadMovieById(request.Id, cancellationToken);

        NotFoundException.ThrowIfNull(result, EntityType.Movie);

        return result;
    }
}
