namespace CleanMinimalApi.Application.Movies.GetMovies;

using CleanMinimalApi.Application.Entities;
using MediatR;

public class GetMoviesQuery : IRequest<List<Movie>>
{
}
