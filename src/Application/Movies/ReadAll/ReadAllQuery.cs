namespace CleanMinimalApi.Application.Movies.ReadAll;

using Entities;
using MediatR;

public class ReadAllQuery : IRequest<List<Movie>>
{
}
