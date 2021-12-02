namespace CleanMinimalApi.Application.Movies.ReadById;

using CleanMinimalApi.Application.Entities;
using MediatR;

public class ReadByIdQuery : IRequest<Movie>
{
    public Guid Id { get; set; }
}
