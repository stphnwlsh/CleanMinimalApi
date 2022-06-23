namespace CleanMinimalApi.Application.Authors.ReadById;

using Entities;
using MediatR;

public class ReadByIdQuery : IRequest<Author>
{
    public Guid Id { get; set; }
}
