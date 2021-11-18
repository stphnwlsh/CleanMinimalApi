namespace CleanMinimalApi.Application.Reviews.ReadById;

using CleanMinimalApi.Domain.Reviews.Entities;
using MediatR;

public class ReadByIdQuery : IRequest<Review>
{
    public Guid Id { get; set; }
}
