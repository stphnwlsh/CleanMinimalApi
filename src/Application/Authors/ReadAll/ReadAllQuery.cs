namespace CleanMinimalApi.Application.Authors.ReadAll;

using CleanMinimalApi.Application.Entities;
using MediatR;

public class ReadAllQuery : IRequest<List<Author>>
{
}
