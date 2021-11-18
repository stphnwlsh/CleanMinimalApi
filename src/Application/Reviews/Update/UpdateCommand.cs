namespace CleanMinimalApi.Application.Reviews.Update;

using MediatR;

public class UpdateCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public Guid AuthorId { get; set; }
    public Guid MovieId { get; set; }
    public int Stars { get; set; }
}
