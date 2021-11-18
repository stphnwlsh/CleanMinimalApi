namespace CleanMinimalApi.Application.Movies.ReadAll;

using CleanMinimalApi.Domain.Movies.Entities;
using MediatR;

public class ReadAllQuery : IRequest<List<Movie>>
{
}
