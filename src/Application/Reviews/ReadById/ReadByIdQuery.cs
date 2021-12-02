namespace CleanMinimalApi.Application.Reviews.ReadById;

using CleanMinimalApi.Application.Entities;
using MediatR;

public class ReadByIdQuery : IRequest<Review>
{
    public Guid Id { get; set; }
}
