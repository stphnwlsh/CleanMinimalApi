namespace CleanMinimalApi.Application.Movies.Create;

using Entities;
using MediatR;

public class CreateCommand : IRequest<Review>
{
    public Guid AuthorId { get; set; }
    public Guid MovieId { get; set; }
    public int Stars { get; set; }
}