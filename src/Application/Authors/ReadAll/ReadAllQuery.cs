namespace CleanMinimalApi.Application.Authors.ReadAll;

using CleanMinimalApi.Domain.Authors.Entities;
using MediatR;

public class ReadAllQuery : IRequest<List<Author>>
{
}
