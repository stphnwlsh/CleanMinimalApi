namespace CleanMinimalApi.Application.Authors.ReadAll;

using Entities;
using MediatR;

public class ReadAllQuery : IRequest<List<Author>>
{
}
