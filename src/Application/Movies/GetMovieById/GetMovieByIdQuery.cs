namespace CleanMinimalApi.Application.Movies.GetMovieById;

using CleanMinimalApi.Application.Entities;
using MediatR;

public class GetMovieByIdQuery : IRequest<Movie>
{
    public Guid Id { get; set; }
}
