namespace CleanMinimalApi.Application.Authors.ReadById;

using CleanMinimalApi.Domain.Authors.Entities;
using MediatR;

public class ReadByIdQuery : IRequest<Author>
{
    public Guid Id { get; set; }
}
