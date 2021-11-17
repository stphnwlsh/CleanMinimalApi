namespace CleanMinimalApi.Application.Reviews.ReadAll;

using CleanMinimalApi.Domain.Reviews.Entities;
using MediatR;

public class ReadAllQuery : IRequest<List<Review>>
{
}
