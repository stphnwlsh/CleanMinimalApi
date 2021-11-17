namespace CleanMinimalApi.Application.Movies.ReadById;

using CleanMinimalApi.Domain.Movies.Entities;
using MediatR;

public class ReadByIdQuery : IRequest<Movie>
{
    public Guid Id { get; set; }
}
