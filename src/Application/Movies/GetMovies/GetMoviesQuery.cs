namespace CleanMinimalApi.Application.Movies.GetMovies;

using Entities;
using MediatR;

public class GetMoviesQuery : IRequest<List<Movie>>
{
}
