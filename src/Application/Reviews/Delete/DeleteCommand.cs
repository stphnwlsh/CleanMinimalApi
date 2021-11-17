namespace CleanMinimalApi.Application.Reviews.Delete;

using MediatR;

public class DeleteCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}
