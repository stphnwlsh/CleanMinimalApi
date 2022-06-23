namespace CleanMinimalApi.Application.Reviews.ReadAll;

using Entities;
using MediatR;

public class ReadAllQuery : IRequest<List<Review>>
{
}
