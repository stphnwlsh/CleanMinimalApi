namespace CleanMinimalApi.Application.Reviews.ReadAll;

using CleanMinimalApi.Application.Entities;
using MediatR;

public class ReadAllQuery : IRequest<List<Review>>
{
}
