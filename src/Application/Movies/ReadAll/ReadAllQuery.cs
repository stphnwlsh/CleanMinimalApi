namespace CleanMinimalApi.Application.Movies.ReadAll;

using CleanMinimalApi.Application.Entities;
using MediatR;

public class ReadAllQuery : IRequest<List<Movie>>
{
}
